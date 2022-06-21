﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestModuleAttribute : Attribute, IModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) => serviceCollection
            .AddContextualLog()
            .AddSingleton<Counter>()
            .AddScoped<IFixtureSetUp, TestTimeLogger>();
    }
}
