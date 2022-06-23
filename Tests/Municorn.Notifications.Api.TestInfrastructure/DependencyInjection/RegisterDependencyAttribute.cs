using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class RegisterDependencyAttribute : Attribute
    {
        public RegisterDependencyAttribute(Type implementationType) => this.ImplementationType = implementationType;

        public RegisterDependencyAttribute()
        {
        }

        public Type? ImplementationType { get; }
    }
}
