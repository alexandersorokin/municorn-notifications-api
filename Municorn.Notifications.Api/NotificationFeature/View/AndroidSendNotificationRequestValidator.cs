using FluentValidation;

namespace Municorn.Notifications.Api.NotificationFeature.View
{
    public class AndroidSendNotificationRequestValidator : AbstractValidator<AndroidSendNotificationRequest>
    {
        public AndroidSendNotificationRequestValidator()
        {
            this.RuleFor(request => request.DeviceToken).MaximumLength(50);
            this.RuleFor(request => request.Message).MaximumLength(2000);
            this.RuleFor(request => request.Title).MaximumLength(255);
        }
    }
}
