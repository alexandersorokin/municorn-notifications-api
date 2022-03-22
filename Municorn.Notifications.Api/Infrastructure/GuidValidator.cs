using System;
using FluentValidation;

namespace Municorn.Notifications.Api.Infrastructure
{
    public class GuidValidator : AbstractValidator<Guid>
    {
        public GuidValidator()
        {
            this.RuleFor(guid => guid).NotEmpty();
        }
    }
}
