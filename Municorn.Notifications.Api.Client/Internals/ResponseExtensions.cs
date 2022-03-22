using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Vostok.Clusterclient.Core.Model;

namespace Municorn.Notifications.Api.Internals
{
    internal static class ResponseExtensions
    {
        internal static async Task<TValue> ConvertBody<TValue>(
            this Response response,
            Func<Stream, Task<TValue>> convert,
            TValue noBodyValue)
        {
            if (response.TryGetContentStream(out var responseStream))
            {
                await using (responseStream.ConfigureAwait(false))
                {
                    return await convert(responseStream).ConfigureAwait(false);
                }
            }

            return noBodyValue;
        }

        private static bool TryGetContentStream(
            this Response response,
            [MaybeNullWhen(false)] out Stream stream)
        {
            if (response.HasContent)
            {
                stream = response.Content.ToMemoryStream();
                return true;
            }

            if (response.HasStream)
            {
                stream = response.Stream;
                return true;
            }

            stream = default;
            return false;
        }
    }
}
