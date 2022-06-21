using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    internal class UseContainerMethodInfo : MethodWrapper, IMethodInfo
    {
        private readonly IServiceProvider serviceProvider;
        private readonly object containerFixture;
        private readonly IMethodInfo methodInfo;

        public UseContainerMethodInfo(IMethodInfo methodInfo, IServiceProvider serviceProvider, object containerFixture)
            : base(methodInfo.TypeInfo.Type, methodInfo.MethodInfo)
        {
            this.methodInfo = methodInfo;
            this.serviceProvider = serviceProvider;
            this.containerFixture = containerFixture;
        }

        object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
        {
            var resolvedArguments = this.ResolveArgs(fixture, args ?? Enumerable.Empty<object?>()).ToArray();
            return this.methodInfo.Invoke(fixture, resolvedArguments);
        }

        private IEnumerable<object?> ResolveArgs(object? methodFixture, IEnumerable<object?> args)
        {
            return
                from arg in args
                let serviceType = (arg as IInjectedService)?.GetServiceType(methodFixture, this.containerFixture)
                select serviceType is null ? arg : this.serviceProvider.GetRequiredService(serviceType);
        }
    }
}