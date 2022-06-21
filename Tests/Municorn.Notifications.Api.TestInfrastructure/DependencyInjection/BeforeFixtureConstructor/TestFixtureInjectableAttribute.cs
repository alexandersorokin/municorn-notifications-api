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
        private readonly object?[] arguments;

        public TestFixtureInjectableAttribute(params object?[] arguments) => this.arguments = arguments;

        public TestFixtureInjectableAttribute()
            : this(Array.Empty<object>())
        {
        }

        public Type[] TypeArgs { get; set; } = Array.Empty<Type>();

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) => this.CreateImplementation().BuildFrom(CreateWrapper(typeInfo));

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) => this.CreateImplementation().BuildFrom(CreateWrapper(typeInfo), filter);

        private static TypeInfoWrapper CreateWrapper(ITypeInfo typeInfo) => new(typeInfo.Type);

        private TestFixtureAttribute CreateImplementation() => new(this.arguments)
        {
            TypeArgs = this.TypeArgs,
        };
    }
}