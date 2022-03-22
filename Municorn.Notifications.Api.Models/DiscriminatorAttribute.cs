using System;

namespace Municorn.Notifications.Api
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DiscriminatorAttribute : Attribute
    {
        public DiscriminatorAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}
