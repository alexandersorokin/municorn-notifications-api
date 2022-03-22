using FluentValidation;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    public class IosSendNotificationRequestValidator : AbstractValidator<IosSendNotificationRequest>
    {
        public IosSendNotificationRequestValidator()
        {
            this.RuleFor(request => request.PushToken).MaximumLength(50);
            this.RuleFor(request => request.Alert).MaximumLength(2000);
        }
    }
}
