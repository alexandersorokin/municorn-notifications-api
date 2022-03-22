using System;

namespace Municorn.Notifications.Api
{
    [AttributeUsage(AttributeTargets.Class)]
    [PrimaryConstructor]
    public sealed partial class DiscriminatorAttribute : Attribute
    {
        public string Value { get; }
    }
}
