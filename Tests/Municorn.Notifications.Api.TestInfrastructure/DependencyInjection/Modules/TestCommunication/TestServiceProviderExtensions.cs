using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication
{
    public static class TestServiceProviderExtensions
    {
        private const string Key = "_private_FixtureServiceProviderMap_Do_Not_ReadDireclty_Use_" + nameof(TestServiceProviderExtensions);

        public static IServiceProvider GetServiceProvider(this ITest test, object fixture) => GetFixtureServiceProviderMap(test).Get(fixture);

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