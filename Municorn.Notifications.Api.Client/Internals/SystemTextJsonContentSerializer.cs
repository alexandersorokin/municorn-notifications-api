using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kontur.Results;
using Microsoft.AspNetCore.Mvc;

namespace Municorn.Notifications.Api.Internals
{
    internal class SystemTextJsonContentSerializer : IContentSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            IgnoreNullValues = true,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new PolymorphicConverter<SendNotificationRequest>(),
                new JsonStringEnumConverter(),
            },
        };

        private static readonly ProblemDetailsFactory ProblemDetailsNull = new(
            ClientErrorCodes.ContentNotExists,
            "Deserialization failed",
            "Deserialization result is null");

        public string ContentType => "application/json";

        public async Task<Result<ProblemDetailsFactory, TResult>> Deserialize<TResult>(Stream contentStream)
        {
            try
            {
                var deserializeResult = await JsonSerializer
                    .DeserializeAsync<TResult>(contentStream, Options)
                    .ConfigureAwait(false);
                return deserializeResult ?? Result<ProblemDetailsFactory, TResult>.Fail(ProblemDetailsNull);
            }
            catch (JsonException exception)
            {
                return new ProblemDetailsFactory(
                    ClientErrorCodes.DeserializationFailed,
                    "Deserialization failed. " + exception.Message,
                    "Deserialization failed");
            }
        }

        public async Task<Result<ProblemDetails>> Serialize<TValue>(Stream stream, TValue value)
        {
            try
            {
                await JsonSerializer.SerializeAsync(stream, value, Options).ConfigureAwait(false);
                return Result.Succeed();
            }
            catch (JsonException exception)
            {
                return new ProblemDetails
                {
                    Type = ClientErrorCodes.SerializationFailed,
                    Title = "Serialization failed",
                    Detail = "Serialization failed. " + exception.Message,
                };
            }
        }
    }
}
