using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [PrimaryConstructor]
    internal sealed partial class UseContainerMethodInfo : MethodInfoDecorator, IMethodInfo
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
        {
            var resolvedArguments = this.ResolveArgs(fixture, args ?? Enumerable.Empty<object?>()).ToArray();
            return this.Invoke(fixture, resolvedArguments);
        }

        private IEnumerable<object?> ResolveArgs(object? methodFixture, IEnumerable<object?> args)
        {
            return
                from arg in args
                let serviceType = (arg as IInjectedService)?.GetServiceType(methodFixture, this.fixtureProvider.Fixture)
                select serviceType is null ? arg : this.serviceProvider.GetRequiredService(serviceType);
        }
    }
}