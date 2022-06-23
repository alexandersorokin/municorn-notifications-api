﻿using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communication
{
    internal sealed class MapProviderSaver : IFixtureOneTimeSetUp, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;
        private readonly FixtureServiceProviderMap map;

        public MapProviderSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, ITest test)
        {
            this.serviceProvider = serviceProvider;
            this.fixtureProvider = fixtureProvider;
            this.map = test.GetFixtureServiceProviderMap();
        }

        public void Run() => this.map.AddScope(this.fixtureProvider.Fixture, this.serviceProvider);

        public void Dispose() => this.map.RemoveScope(this.fixtureProvider.Fixture);
    }
}