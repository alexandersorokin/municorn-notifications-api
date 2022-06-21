﻿using System.Collections;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.Source
{
    internal class StandardSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new TestFixtureData(typeof(int), "passed");
        }
    }
}