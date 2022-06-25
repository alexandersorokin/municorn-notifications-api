﻿using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [PrimaryConstructor]
    internal partial class UseContainerMethodInfoFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        public UseContainerMethodInfo Create(IMethodInfo methodInfo) =>
            new(methodInfo, this.serviceProvider, this.fixtureProvider);
    }
}