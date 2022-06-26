using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder.Decorators;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TestFixtureSourceInjectableAttribute : NUnitAttribute, IFixtureBuilder2
    {
        public TestFixtureSourceInjectableAttribute(Type sourceType, string sourceName)
        {
            this.SourceType = sourceType;
            this.SourceName = sourceName;
        }

        public TestFixtureSourceInjectableAttribute(string sourceName) => this.SourceName = sourceName;

        public TestFixtureSourceInjectableAttribute(Type sourceType) => this.SourceType = sourceType;

        public string? SourceName { get; }

        public Type? SourceType { get; }

        public string? Category { get; init; }

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) =>
            this.CreateImplementation(typeInfo)
                .SelectMany(p => p.Implementatation.BuildFrom(p.NewTypeInfo));

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) =>
            this.CreateImplementation(typeInfo)
                .SelectMany(p => p.Implementatation.BuildFrom(p.NewTypeInfo, filter));

        private IEnumerable<(ITypeInfo NewTypeInfo, IFixtureBuilder2 Implementatation)> CreateImplementation(ITypeInfo typeInfo)
        {
            var sourceAttribute = new TestFixtureSourceAttribute(this.SourceName!)
            {
                Category = this.Category,
            };
            var type = typeInfo.Type;
            var testFixtureDatas = sourceAttribute.GetParametersFor(this.SourceType ?? type);

            foreach (var testFixtureData in testFixtureDatas)
            {
                if (testFixtureData.RunState != RunState.Runnable)
                {
                    yield return (typeInfo, new FailureFixtureBuilder(testFixtureData));
                    continue;
                }

                var typeArgs = testFixtureData.TypeArgs ?? Array.Empty<Type>();
                TypeInfoDecorator wrapper = new(type, testFixtureData.Arguments, typeArgs);
                TestFixtureAttribute implementation = new(testFixtureData.Arguments)
                {
                    TypeArgs = typeArgs,
                    TestName = testFixtureData.TestName,
                };

                yield return (wrapper, implementation);
            }
        }

        private class FailureFixtureBuilder : IFixtureBuilder2
        {
            private readonly ITestFixtureData testFixtureData;

            public FailureFixtureBuilder(ITestFixtureData testFixtureData)
            {
                this.testFixtureData = testFixtureData;
            }

            public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
            {
                var properties = this.testFixtureData.Properties;
                StringBuilder reason = new();
                if (properties.ContainsKey(PropertyNames.SkipReason))
                {
                    reason.AppendLine(properties.Get(PropertyNames.SkipReason) as string ?? string.Empty);
                }

                if (properties.ContainsKey(PropertyNames.ProviderStackTrace))
                {
                    reason.AppendLine(properties.Get(PropertyNames.ProviderStackTrace) as string ?? string.Empty);
                }

                TestFixture testFixture = new(typeInfo);
                testFixture.MakeInvalid(reason.ToString());
                yield return testFixture;
            }

            public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) => this.BuildFrom(typeInfo);
        }
    }
}
