using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ObjectAssertionsExtensions
    {
        internal static void HaveStatusCode(this ObjectAssertions objectAssertions, int statusCode) =>
            objectAssertions.BeEquivalentTo(
                new ProblemDetails { Status = statusCode },
                config => config.Including(p => p.Status));

        internal static void BeBadRequest(this ObjectAssertions objectAssertions) => objectAssertions.HaveStatusCode(400);
    }
}
