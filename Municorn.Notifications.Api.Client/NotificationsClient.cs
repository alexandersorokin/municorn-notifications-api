using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kontur.Results;
using Microsoft.AspNetCore.Mvc;
using Municorn.Notifications.Api.Internals;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Model;
using Vostok.Clusterclient.Transport;

namespace Municorn.Notifications.Api
{
    public class NotificationsClient : INotificationsClient
    {
        private static readonly IContentSerializer Serializer = new SystemTextJsonContentSerializer();
        private static readonly ClusterResultDeserializer Deserializer = new(Serializer);

        private readonly Lazy<IClusterClient> client;

        public NotificationsClient(NotificationsClientOptions options)
        {
            var (clientTopology, log) = options;
            this.client = new(() =>
            {
                return new ClusterClient(
                    log,
                    configuration =>
                    {
                        clientTopology.Setup(configuration);
                        configuration.SetupUniversalTransport();
                    });
            });
        }

        [MustUseReturnValue]
        public async Task<Result<ProblemDetails, SendResponse>> SendNotificationAsync(SendNotificationRequest notification)
        {
            var serializationResult = await Serialize(notification).ConfigureAwait(false);
            if (serializationResult.TryGetFault(out var problem, out var content))
            {
                return problem;
            }

            var uri = new Uri(NotificationsApiUris.Notifications, UriKind.Relative);
            var request = new Request(RequestMethods.Post, uri)
                .WithContent(content)
                .WithContentTypeHeader(Serializer.ContentType);
            return await this.SendRequest<SendResponse>(request).ConfigureAwait(false);
        }

        [MustUseReturnValue]
        public async Task<Result<ProblemDetails, SendStatusResponse>> GetNotificationStatusAsync(Guid notificationId)
        {
            var uri = new Uri($"{NotificationsApiUris.Notifications}/{notificationId}/status", UriKind.Relative);
            var request = new Request(RequestMethods.Get, uri);
            return await this.SendRequest<SendStatusResponse>(request).ConfigureAwait(false);
        }

        private static async Task<Result<ProblemDetails, ArraySegment<byte>>> Serialize(SendNotificationRequest notification)
        {
            using var stream = new MemoryStream();
            var serializationResult = await Serializer.Serialize(stream, notification).ConfigureAwait(false);
            return serializationResult.TryGetFault(out var serializationProblem)
                ? serializationProblem
                : new ArraySegment<byte>(stream.GetBuffer(), 0, (int)stream.Length);
        }

        private async Task<Result<ProblemDetails, TResult>> SendRequest<TResult>(Request request)
        {
            var jsonRequest = request
                .WithAcceptHeader(Serializer.ContentType)
                .WithAcceptCharsetHeader(Encoding.UTF8);
            var response = await this.client.Value.SendAsync(jsonRequest).ConfigureAwait(false);
            return await Deserializer.Deserialize<TResult>(response).ConfigureAwait(false);
        }
    }
}