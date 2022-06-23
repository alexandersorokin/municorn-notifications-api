using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class RegisterDependencyAttribute : Attribute
    {
        public Type? ImplementationType { get; init; }
    }
}
