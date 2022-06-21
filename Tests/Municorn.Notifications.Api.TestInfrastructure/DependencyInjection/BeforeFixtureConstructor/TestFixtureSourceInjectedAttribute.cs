using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TestFixtureSourceInjectedAttribute : NUnitAttribute, IFixtureBuilder2
    {
        public TestFixtureSourceInjectedAttribute(string sourceName)
        {
            this.SourceName = sourceName;
        }

        public TestFixtureSourceInjectedAttribute(Type sourceType, string sourceName)
        {
            this.SourceType = sourceType;
            this.SourceName = sourceName;
        }

        public TestFixtureSourceInjectedAttribute(Type sourceType)
        {
            this.SourceType = sourceType;
        }

        public string? SourceName { get; }

        public Type? SourceType { get; }

        public string? Category { get; set; }

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo) =>
            this.CreateImplementation(typeInfo)
                .SelectMany(p => p.Implementatation.BuildFrom(p.Wrapper));

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter) =>
            this.CreateImplementation(typeInfo)
                .SelectMany(p => p.Implementatation.BuildFrom(p.Wrapper, filter));

        private IEnumerable<(TypeInfoWrapper Wrapper, TestFixtureAttribute Implementatation)> CreateImplementation(ITypeInfo typeInfo)
        {
            var sourceAttribute = new TestFixtureSourceAttribute(this.SourceName!)
            {
                Category = this.Category,
            };
            var type = typeInfo.Type;
            var testFixtureDatas = sourceAttribute.GetParametersFor(this.SourceType ?? type);

            foreach (var testFixtureData in testFixtureDatas)
            {
                var typeArgs = testFixtureData.TypeArgs ?? Array.Empty<Type>();
                TypeInfoWrapper wrapper = new(type, testFixtureData.Arguments, typeArgs);
                TestFixtureAttribute implementatation = new(testFixtureData.Arguments)
                {
                    TypeArgs = typeArgs,
                    TestName = testFixtureData.TestName,
                };

                yield return (wrapper, implementatation);
            }
        }
    }
}
