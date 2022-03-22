using System;
using System.Reflection;
using FluentValidation.Validators;
using MicroElements.Swashbuckle.FluentValidation;

namespace Municorn.Notifications.Api.Infrastructure.Swagger
{
    internal class NotEmptyFluentValidationRule : FluentValidationRule
    {
        public NotEmptyFluentValidationRule()
            : base(
                "NotEmpty",
                new Func<IPropertyValidator, bool>[]
                {
                    propertyValidator => propertyValidator is INotEmptyValidator,
                },
                ApplyInner)
        {
        }

        private static void ApplyInner(RuleContext context)
        {
            context.Property.Nullable = false;

            var propertyName = context.PropertyKey;
            var required = context.Schema.Required;
            if (!required.Contains(propertyName))
            {
                required.Add(propertyName);
            }

            switch (context.Property.Type)
            {
                case "array":
                    context.Property.MinItems = Math.Max(1, context.Property.MinItems ?? 0);
                    return;
                case "object":
                    context.Property.MinProperties = Math.Max(1, context.Property.MinProperties ?? 0);
                    return;
            }

            if (context.ReflectionContext.PropertyInfo is PropertyInfo propertyInfo &&
                propertyInfo.PropertyType == typeof(string))
            {
                context.Property.MinLength = Math.Max(1, context.Property.MinLength ?? 0);
            }
        }
    }
}
