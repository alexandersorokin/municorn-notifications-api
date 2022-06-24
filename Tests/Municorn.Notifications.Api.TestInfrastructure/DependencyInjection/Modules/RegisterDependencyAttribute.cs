using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public sealed class RegisterDependencyAttribute : Attribute
    {
        public RegisterDependencyAttribute(Type implementationType) => this.ImplementationType = implementationType;

        public RegisterDependencyAttribute()
        {
        }

        public Type? ImplementationType { get; }
    }
}
