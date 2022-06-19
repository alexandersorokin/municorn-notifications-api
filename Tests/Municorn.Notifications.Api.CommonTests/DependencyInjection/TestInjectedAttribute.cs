using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    internal sealed class TestInjectedAttribute : NUnitAttribute, ISimpleTestBuilder
    {
        public TestMethod BuildFrom(IMethodInfo method, Test? suite)
        {
            var methodWrapper = new MethodWrapper(method);
            var testMethod = new TestAttribute().BuildFrom(methodWrapper, suite);
            methodWrapper.Test = testMethod;
            return testMethod;
        }

        private class MethodWrapper : IMethodInfo
        {
            private readonly IMethodInfo implementation;

            public MethodWrapper(IMethodInfo implementation) => this.implementation = implementation;

            internal ITest Test { private get; set; } = default!;

            public T[] GetCustomAttributes<T>(bool inherit)
                where T : class =>
                this.implementation.GetCustomAttributes<T>(inherit);

            public bool IsDefined<T>(bool inherit)
                where T : class =>
                this.implementation.IsDefined<T>(inherit);

            public IParameterInfo[] GetParameters() => this.implementation
                .GetParameters()
                .Select(parameter => new ParameterOptionalWrapper(parameter))
                .ToArray<IParameterInfo>();

            public Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

            public IMethodInfo MakeGenericMethod(params Type[] typeArguments) => this.implementation.MakeGenericMethod(typeArguments);

            public object? Invoke(object? fixture, params object?[]? args) =>
                fixture is IConfigureServices configureServices
                    ? this.implementation.Invoke(configureServices, this.ResolveArgs(configureServices))
                    : throw new InvalidOperationException($"{nameof(TestInjectedAttribute)} can be only applied to {nameof(IConfigureServices)} fixture");

            private object?[] ResolveArgs(IConfigureServices fixture)
            {
                var serviceProvider = this.Test.GetFixtureServiceProviderMap().GetScope(fixture).ServiceProvider;
                return this.implementation.GetParameters()
                    .Select(parameter => serviceProvider.GetRequiredService(parameter.ParameterType))
                    .ToArray();
            }

            public ITypeInfo TypeInfo => this.implementation.TypeInfo;

            public MethodInfo MethodInfo => this.implementation.MethodInfo;

            public string Name => this.implementation.Name;

            public bool IsAbstract => this.implementation.IsAbstract;

            public bool IsPublic => this.implementation.IsPublic;

            public bool IsStatic => this.implementation.IsStatic;

            public bool ContainsGenericParameters => this.implementation.ContainsGenericParameters;

            public bool IsGenericMethod => this.implementation.IsGenericMethod;

            public bool IsGenericMethodDefinition => this.implementation.IsGenericMethodDefinition;

            public ITypeInfo ReturnType => this.implementation.ReturnType;

            private class ParameterOptionalWrapper : IParameterInfo
            {
                private readonly IParameterInfo implementation;

                public ParameterOptionalWrapper(IParameterInfo implementation) => this.implementation = implementation;

                public T[] GetCustomAttributes<T>(bool inherit)
                    where T : class =>
                    this.implementation.GetCustomAttributes<T>(inherit);

                public bool IsDefined<T>(bool inherit)
                    where T : class =>
                    this.implementation.IsDefined<T>(inherit);

                public bool IsOptional => true;

                public IMethodInfo Method => this.implementation.Method;

                public ParameterInfo ParameterInfo => this.implementation.ParameterInfo;

                public Type ParameterType => this.implementation.ParameterType;
            }
        }
    }
}