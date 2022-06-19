using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixture
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestFixtureInjectedAttribute : NUnitAttribute, IFixtureBuilder2
    {
        private readonly TestFixtureAttribute implementation = new();

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
        {
            return this.implementation.BuildFrom(new TypeInfoWrapper(typeInfo));
        }

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) =>
            this.implementation.BuildFrom(new TypeInfoWrapper(typeInfo), filter);
    }
}