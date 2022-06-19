using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kontur.Results;
using Microsoft.AspNetCore.Mvc;
using Vostok.Clusterclient.Core.Model;

namespace Municorn.Notifications.Api.Internals
{
    [PrimaryConstructor]
    internal partial class ClusterResultDeserializer
    {
        private static readonly IReadOnlySet<ClusterResultStatus> ServerResponseStatuses = new HashSet<ClusterResultStatus>
        {
            ClusterResultStatus.Success,
            ClusterResultStatus.ReplicasExhausted,
        };

        private static readonly IReadOnlySet<ResponseCode> GatewayCodes = new HashSet<ResponseCode>
        {
            ResponseCode.ProxyAuthenticationRequired,
            ResponseCode.ProxyTimeout,
            ResponseCode.ServiceUnavailable,
        };

        private static readonly ProblemDetailsFactory NoBodyServiceProblemFactory = new(
            ClientErrorCodes.ContentNotExists,
            "Deserialization failed",
            "No content was sent by server for deserialization");

        private static readonly ProblemDetailsFactory NotBodyGatewayProblem = new(
            ClientErrorCodes.UnavailableError,
            "No body in gateway response",
            "No content was sent by gateway");

        private readonly IContentSerializer contentSerializer;

        public async Task<Result<ProblemDetails, TResponse>> Deserialize<TResponse>(ClusterResult clusterResult)
        {
            var maybeProblem = await this.GetClusterSuccessfulResponse(clusterResult).ConfigureAwait(false);
            if (maybeProblem.TryGetFault(out var problem, out var response))
            {
                return problem;
            }

            var responseCode = response.Code;
            if (responseCode == ResponseCode.NoContent)
            {
                return new ProblemDetailsFactory(
                        ClientErrorCodes.ContentNotExists,
                        "Deserialization failed",
                        $"No content was sent by server for deserialization. Status code {responseCode}")
                    .Create(responseCode);
            }

            var result = await this.ReadResponse<TResponse>(response).ConfigureAwait(false);
            return result.MapFault(factory => factory.Create(responseCode));
        }

        private static async Task<ProblemDetailsFactory> GetGatewayProblem(Response response) =>
            await response.ConvertBody(
                async stream =>
                {
                    using var reader = new StreamReader(stream);
                    var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                    return new(
                        ClientErrorCodes.UnavailableError,
                        "Gateway send response",
                        $"Result sent by gateway. {body}");
                },
                NotBodyGatewayProblem).ConfigureAwait(false);

        private static string GetClientErrorCode(ClusterResultStatus status) =>
            status switch
            {
                ClusterResultStatus.ReplicasExhausted => ClientErrorCodes.UnavailableError,
                ClusterResultStatus.ReplicasNotFound => ClientErrorCodes.UnavailableError,
                ClusterResultStatus.Throttled => ClientErrorCodes.ThrottledError,
                ClusterResultStatus.TimeExpired => ClientErrorCodes.TimeoutError,
                _ => ClientErrorCodes.Error,
            };

        private async Task<Result<ProblemDetails, Response>> GetClusterSuccessfulResponse(ClusterResult clusterResult)
        {
            if (ServerResponseStatuses.Contains(clusterResult.Status))
            {
                return await this.GetServerProblem(clusterResult.Response).ConfigureAwait(false);
            }

            return new ProblemDetails
            {
                Type = GetClientErrorCode(clusterResult.Status),
                Title = "Request to server failed",
                Detail = $"Request to server failed. Cluster result status: {clusterResult.Status}",
            };
        }

        private async Task<Result<ProblemDetails, Response>> GetServerProblem(Response response)
        {
            var responseCode = response.Code;
            if (responseCode.IsSuccessful())
            {
                return response;
            }

            if (responseCode.IsClientError() || responseCode.IsServerError())
            {
                if (GatewayCodes.Contains(responseCode))
                {
                    var problemDetailsFactory = await GetGatewayProblem(response).ConfigureAwait(false);
                    return problemDetailsFactory.Create(responseCode);
                }

                var serviceProblem = await this.GetServiceProblem(response).ConfigureAwait(false);
                return serviceProblem.Match(
                    factory => factory.Create(responseCode),
                    problem => problem);
            }

            return new ProblemDetailsFactory(
                ClientErrorCodes.Error,
                "Unexpected server response",
                $"Unexpected server response. Code {responseCode}")
                .Create(responseCode);
        }

        private async Task<Result<ProblemDetailsFactory, ProblemDetails>> GetServiceProblem(Response response) => await this.ReadResponse<ProblemDetails>(response).ConfigureAwait(false);

        private async Task<Result<ProblemDetailsFactory, TValue>> ReadResponse<TValue>(Response response) =>
            await response.ConvertBody(
                async stream => await this.contentSerializer.Deserialize<TValue>(stream).ConfigureAwait(false),
                NoBodyServiceProblemFactory).ConfigureAwait(false);
    }
}
