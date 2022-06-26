using System;
using System.Collections.Generic;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder.Decorators;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TestFixtureInjectableAttribute : NUnitAttribute, IFixtureBuilder2
    {
        private readonly object?[] arguments;

        public TestFixtureInjectableAttribute(params object?[] arguments) => this.arguments = arguments;

        public TestFixtureInjectableAttribute()
            : this(Array.Empty<object>())
        {
        }

        public Type[] TypeArgs { get; init; } = Array.Empty<Type>();

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) =>
            this.CreateImplementation().BuildFrom(this.CreateTypeWrapperDecorator(typeInfo));

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) =>
            this.CreateImplementation().BuildFrom(this.CreateTypeWrapperDecorator(typeInfo), filter);

        private TypeWrapperDecorator CreateTypeWrapperDecorator(ITypeInfo typeInfo) =>
            new(typeInfo, this.arguments, this.TypeArgs);

        private TestFixtureAttribute CreateImplementation() => new(this.arguments)
        {
            TypeArgs = this.TypeArgs,
        };
    }
}