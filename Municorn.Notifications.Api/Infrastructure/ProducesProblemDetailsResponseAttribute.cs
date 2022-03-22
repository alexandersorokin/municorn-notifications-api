using Microsoft.AspNetCore.Mvc;

namespace Municorn.Notifications.Api.Infrastructure
{
    public sealed class ProducesProblemDetailsResponseAttribute : ProducesResponseTypeAttribute
    {
        public ProducesProblemDetailsResponseAttribute(int statusCode)
            : base(typeof(ProblemDetails), statusCode)
        {
        }
    }
}
