using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    public static class TestServiceProviderExtensions
    {
        private const string Key = "FixtureServiceProviderMap";

        public static IServiceProvider GetServiceProvider(this ITest test, object fixture) => GetFixtureServiceProviderMap(test).GetScope(fixture);

        internal static FixtureServiceProviderMap GetFixtureServiceProviderMap(this ITest test)
        {
            var properties = test.Properties;
            if (properties.Get(Key) is FixtureServiceProviderMap alreadyCreatedValue)
            {
                return alreadyCreatedValue;
            }

            FixtureServiceProviderMap map = new();
            properties.Set(Key, map);
            return map;
        }
    }
}