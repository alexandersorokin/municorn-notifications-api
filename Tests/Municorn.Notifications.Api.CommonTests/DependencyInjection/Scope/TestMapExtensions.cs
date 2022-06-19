using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal static class TestMapExtensions
    {
        private const string Key = "FixtureServiceProviderMap";

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