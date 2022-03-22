using System.IO;
using System.Threading.Tasks;
using Kontur.Results;
using Microsoft.AspNetCore.Mvc;

namespace Municorn.Notifications.Api.Internals
{
    internal interface IContentSerializer
    {
        string ContentType { get; }

        Task<Result<ProblemDetailsFactory, TResult>> Deserialize<TResult>(Stream contentStream);

        Task<Result<ProblemDetails>> Serialize<TValue>(Stream stream, TValue value);
    }
}