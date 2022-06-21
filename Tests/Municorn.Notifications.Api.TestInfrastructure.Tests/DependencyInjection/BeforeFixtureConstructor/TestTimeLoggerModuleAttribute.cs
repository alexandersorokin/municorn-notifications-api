﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestTimeLoggerModuleAttribute : Attribute, IModule
    {
        private static readonly LogModuleAttribute LogModule = new();

        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            LogModule.ConfigureServices(
                serviceCollection
                    .AddSingleton<Counter>()
                    .AddScoped<IFixtureSetUp, TestTimeLogger>(),
                typeInfo);
    }
}