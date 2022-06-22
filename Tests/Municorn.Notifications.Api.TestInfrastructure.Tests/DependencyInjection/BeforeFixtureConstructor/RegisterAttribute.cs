using System;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class RegisterAttribute : Attribute
    {
        internal Type? ImplementationType { get; init; }
    }
}
