using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestFixtureInjectableAttribute : NUnitAttribute, IFixtureBuilder2
    {
        private readonly TestFixtureAttribute implementation = new();

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) => this.implementation.BuildFrom(CreateWrapper(typeInfo));

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) => this.implementation.BuildFrom(CreateWrapper(typeInfo), filter);

        private static TypeInfoWrapper CreateWrapper(ITypeInfo typeInfo) => new(typeInfo.Type);
    }
}