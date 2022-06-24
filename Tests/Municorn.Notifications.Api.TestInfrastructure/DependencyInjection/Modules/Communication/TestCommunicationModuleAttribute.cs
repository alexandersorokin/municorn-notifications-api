﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication.AsyncLocal;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Communication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class TestCommunicationModuleAttribute : Attribute, IFixtureModule
    {
        public void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo) =>
            serviceCollection
                .AddSingleton<IAsyncLocalServiceProvider, AsyncLocalServiceProvider>()
                .AddSingleton(typeof(IAsyncLocalServiceProvider<>), typeof(AsyncLocalServiceProvider<>))
                .AddSingleton<IFixtureOneTimeSetUp, MapProviderSaver>()
                .AddScoped<IFixtureSetUp, MapScopeSaver>();
    }
}
