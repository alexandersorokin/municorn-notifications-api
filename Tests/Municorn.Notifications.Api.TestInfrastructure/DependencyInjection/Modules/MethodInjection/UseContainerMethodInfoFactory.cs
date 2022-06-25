﻿using System;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [PrimaryConstructor]
    internal sealed partial class UseContainerMethodInfoFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        public UseContainerMethodInfo Create(IMethodInfo methodInfo) =>
            new(methodInfo, this.serviceProvider, this.fixtureProvider);
    }
}
