using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder.Decorators
{
    internal sealed class TypeWrapperDecorator : ITypeInfo
    {
        private readonly FixtureServiceProviderMap globalServiceProviders = new();
        private readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders = new();
        private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks = new();

        private readonly ITypeInfo originalType;
        private readonly object?[] arguments;
        private readonly Type[] fixtureTypeArgs;
        private readonly Type wrappedType;

        public TypeWrapperDecorator(ITypeInfo originalType, object?[] arguments, Type[] fixtureTypeArgs)
        {
            this.originalType = originalType;
            this.arguments = arguments;
            this.fixtureTypeArgs = fixtureTypeArgs;

            this.wrappedType = new LimitingConstructorParametersTypeDecorator(originalType.Type, arguments);
        }

        ITypeInfo? ITypeInfo.BaseType => this.originalType.BaseType;

        string ITypeInfo.Name => this.originalType.Name;

        string ITypeInfo.FullName => this.originalType.FullName;

        Assembly ITypeInfo.Assembly => this.originalType.Assembly;

        string ITypeInfo.Namespace => this.originalType.Namespace;

        bool ITypeInfo.IsAbstract => this.originalType.IsAbstract;

        bool ITypeInfo.IsGenericType => this.originalType.IsGenericType;

        bool ITypeInfo.ContainsGenericParameters => this.originalType.ContainsGenericParameters;

        bool ITypeInfo.IsGenericTypeDefinition => this.originalType.IsGenericTypeDefinition;

        bool ITypeInfo.IsSealed => this.originalType.IsSealed;

        bool ITypeInfo.IsStaticClass => this.originalType.IsStaticClass;

        bool ITypeInfo.IsType(Type type) => this.originalType.IsType(type);

        string ITypeInfo.GetDisplayName() => this.originalType.GetDisplayName();

        string ITypeInfo.GetDisplayName(object?[]? args) => this.originalType.GetDisplayName(args);

        Type ITypeInfo.GetGenericTypeDefinition() => this.originalType.GetGenericTypeDefinition();

        T[] IReflectionInfo.GetCustomAttributes<T>(bool inherit) => this.originalType.GetCustomAttributes<T>(inherit);

        bool IReflectionInfo.IsDefined<T>(bool inherit) => this.originalType.IsDefined<T>(inherit);

        bool ITypeInfo.HasMethodWithAttribute(Type attrType) => this.originalType.HasMethodWithAttribute(attrType);

        ConstructorInfo? ITypeInfo.GetConstructor(Type[] argTypes) => this.originalType.GetConstructor(argTypes);

        bool ITypeInfo.HasConstructor(Type[] argTypes) => this.originalType.HasConstructor(argTypes);

        Type ITypeInfo.Type => this.wrappedType;

        ITypeInfo ITypeInfo.MakeGenericType(Type[] typeArgs)
        {
            var args = this.fixtureTypeArgs.Any()
                ? this.arguments
                : this.arguments
                    .SkipWhile((arg, i) => i < typeArgs.Length && typeArgs[i].Equals(arg))
                    .ToArray();

            return new TypeWrapperDecorator(
                this.originalType.MakeGenericType(typeArgs),
                args,
                Array.Empty<Type>());
        }

        IMethodInfo[] ITypeInfo.GetMethods(BindingFlags flags) => this
            .originalType
            .GetMethods(flags)
            .Select(methodInfo => new ModifyAttributesTestMethodWrapperDecorator(this.frameworks, methodInfo))
            .ToArray<IMethodInfo>();

        IMethodInfo[] ITypeInfo.GetMethodsWithAttribute<T>(bool inherit)
            where T : class
        {
            var result = this.originalType.GetMethodsWithAttribute<T>(inherit);

            HashSet<Type> fixtureMethods = new()
            {
                typeof(OneTimeSetUpAttribute),
                typeof(OneTimeTearDownAttribute),
            };
            if (fixtureMethods.Contains(typeof(T)))
            {
                result = result
                    .Select(methodInfo => new FixtureOneTimeActionMethodInfo(this.wrappedType, methodInfo.MethodInfo, this.globalServiceProviders))
                    .ToArray<IMethodInfo>();

                if (typeof(T) == typeof(OneTimeTearDownAttribute))
                {
                    var tearDownMethodExample = this.GetType().GetMethod(nameof(this.TearDownMethodExample), BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new InvalidOperationException(nameof(this.TearDownMethodExample) + " is not found");
                    result = result.Prepend(new OneTimeTearDownMethodInfo(this.wrappedType, tearDownMethodExample, this.frameworks)).ToArray();
                }
            }

            HashSet<Type> typeMethods = new()
            {
                typeof(SetUpAttribute),
                typeof(TearDownAttribute),
            };
            if (typeMethods.Contains(typeof(T)))
            {
                result = result
                    .Select(methodInfo => new FixtureActionMethodInfo(this.scopedServiceProviders, methodInfo))
                    .ToArray<IMethodInfo>();
            }

            return result;
        }

        public Task TearDownMethodExample() => Task.CompletedTask;

        object ITypeInfo.Construct(object?[]? args)
        {
            FixtureAccessor fixtureAccessor = new();

            var currentTest = TestExecutionContext.CurrentContext.CurrentTest;

            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddSingleton(fixtureAccessor)
                .AddSingleton(new FixtureFactoryArgs(this.originalType.Type, args ?? Array.Empty<object>()))
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureFactory>()
                .AddSingleton<IFixtureProvider>(fixtureAccessor)
                .AddSingleton<ITest>(currentTest)
                .AddFixtures(currentTest)
                .AddSingleton(this.globalServiceProviders)
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureServiceProviderSaver>()
                .AddSingleton(this.scopedServiceProviders)
                .AddScoped<IFixtureSetUpService, ScopeServiceProviderSaver>()
                .AddFixtureServiceCollectionModuleAttributes(this.originalType.Type));

            try
            {
                framework.RunOneTimeSetUp().GetAwaiter().GetResult();

                var fixture = fixtureAccessor.Fixture;
                this.frameworks.Add(fixture, framework);
                return fixture;
            }
            catch (Exception)
            {
                framework.DisposeAsync().AsTask().GetAwaiter().GetResult();
                throw;
            }
        }

        private static object[] GetServiceArray(IServiceProvider serviceProvider, IEnumerable<ParameterInfo> methodInfo) =>
            methodInfo
                .Select(p => p.ParameterType)
                .Select(type => serviceProvider.GetRequiredService(type))
                .ToArray();

        private record FixtureFactoryArgs(Type FixtureType, object?[] Args);

        private class FixtureAccessor : IFixtureProvider
        {
            private object? fixture;

            public object Fixture
            {
                get => this.fixture ?? throw new InvalidOperationException("Fixture is not yet set");
                internal set => this.fixture = value;
            }
        }

        private sealed class FixtureFactory : IFixtureOneTimeSetUpService
        {
            private readonly IServiceProvider serviceProvider;
            private readonly FixtureAccessor fixtureProvider;
            private readonly FixtureFactoryArgs fixtureFactoryArgs;

            [UsedImplicitly]
            public FixtureFactory(IServiceProvider serviceProvider, FixtureAccessor fixtureProvider, FixtureFactoryArgs fixtureFactoryArgs)
            {
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
                this.fixtureFactoryArgs = fixtureFactoryArgs;
            }

            public void Run()
            {
                var (type, args) = this.fixtureFactoryArgs;
                var fixture = ActivatorUtilities.CreateInstance(this.serviceProvider, type, args!);
                this.fixtureProvider.Fixture = fixture;
            }
        }

        private sealed class FixtureServiceProviderSaver : IFixtureOneTimeSetUpService, IDisposable
        {
            private readonly IServiceProvider serviceProvider;
            private readonly IFixtureProvider fixtureProvider;
            private readonly FixtureServiceProviderMap globalServiceProviders;

            [UsedImplicitly]
            public FixtureServiceProviderSaver(
                IServiceProvider serviceProvider,
                IFixtureProvider fixtureProvider,
                FixtureServiceProviderMap globalServiceProviders)
            {
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
                this.globalServiceProviders = globalServiceProviders;
            }

            public void Run() => this.globalServiceProviders.Add(this.fixtureProvider.Fixture, this.serviceProvider);

            public void Dispose() => this.globalServiceProviders.Remove(this.fixtureProvider.Fixture);
        }

        private sealed class ScopeServiceProviderSaver : IFixtureSetUpService, IDisposable
        {
            private readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders;
            private readonly IServiceProvider serviceProvider;
            private readonly IFixtureProvider fixtureProvider;
            private readonly ITest test;
            private readonly FixtureServiceProviderMap map;

            [UsedImplicitly]
            public ScopeServiceProviderSaver(
                ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders,
                IServiceProvider serviceProvider,
                IFixtureProvider fixtureProvider,
                TestAccessor testAccessor)
            {
                this.scopedServiceProviders = scopedServiceProviders;
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
                this.test = testAccessor.Test;
                this.map = new();

                scopedServiceProviders.Add(this.test, this.map);
            }

            public void Run() => this.map.Add(this.fixtureProvider.Fixture, this.serviceProvider);

            public void Dispose()
            {
                this.map.Remove(this.fixtureProvider.Fixture);
                this.scopedServiceProviders.Remove(this.test);
            }
        }

        private class FixtureActionMethodInfo : MethodInfoDecorator, IMethodInfo
        {
            private readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders;

            public FixtureActionMethodInfo(
                ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders,
                IMethodInfo methodInfo)
                : base(methodInfo) =>
                this.scopedServiceProviders = scopedServiceProviders;

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var test = TestExecutionContext.CurrentContext.CurrentTest;
                var map = this.scopedServiceProviders.TryGetValue(test, out var result)
                        ? result
                        : throw new InvalidOperationException($"Scoped service provider map is not created for test {test.FullName}");
                var serviceProvider = map.Get(fixture ?? throw new InvalidOperationException("Fixture is found for fixture method"));
                return this.Invoke(fixture, GetServiceArray(serviceProvider, this.MethodInfo.GetParameters()));
            }
        }

        private class FixtureOneTimeActionMethodInfo : MethodWrapper, IMethodInfo
        {
            private readonly FixtureServiceProviderMap globalServiceProviders;

            internal FixtureOneTimeActionMethodInfo(Type type, MethodInfo methodInfo, FixtureServiceProviderMap globalServiceProviders)
                : base(type, methodInfo) =>
                this.globalServiceProviders = globalServiceProviders;

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var serviceProvider = this.globalServiceProviders.Get(fixture ?? throw new InvalidOperationException("Fixture is not passed to OneTime action method"));
                return this.Invoke(fixture, GetServiceArray(serviceProvider, this.MethodInfo.GetParameters()));
            }
        }

        private class OneTimeTearDownMethodInfo : MethodWrapper, IMethodInfo
        {
            private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;

            public OneTimeTearDownMethodInfo(
                Type type,
                MethodInfo methodInfo,
                ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks)
                : base(type, methodInfo) =>
                this.frameworks = frameworks;

            object IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var testFixture = fixture ?? throw new InvalidOperationException("Fixture is not passed to container dispose method");
                var serviceProvider = this.frameworks.TryGetValue(testFixture, out var result)
                    ? result
                    : throw new InvalidOperationException($"Service provider for {fixture} fixture is not found");

                if (!this.frameworks.Remove(testFixture))
                {
                    throw new InvalidOperationException($"Service provider for {fixture} fixture is concurrently removed");
                }

                return serviceProvider.DisposeAsync().AsTask();
            }
        }
    }
}