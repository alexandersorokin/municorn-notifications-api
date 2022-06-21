﻿using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    public static class TestMapExtensions
    {
        private const string Key = "FixtureServiceProviderMap";

        public static FixtureServiceProviderMap GetFixtureServiceProviderMap(this ITest test)
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