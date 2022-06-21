﻿using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [SetUpFixture]
    internal class GlobalLog : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddBoundLog();

        [field: TestDependency]
        internal ILog BoundLog { get; } = default!;
    }
}