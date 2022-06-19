using Microsoft.AspNetCore.Mvc;
using Vostok.Clusterclient.Core.Model;

namespace Municorn.Notifications.Api.Internals
{
    [PrimaryConstructor]
    internal partial class ProblemDetailsFactory
    {
        private readonly string type;
        private readonly string title;
        private readonly string detail;

        internal ProblemDetails Create(ResponseCode responseCode) =>
            new()
            {
                Type = this.type,
                Status = (int)responseCode,
                Title = this.title,
                Detail = this.detail,
            };
    }
}
