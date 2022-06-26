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
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder.Decorators
{
    internal sealed class TypeWrapperDecorator : TypeWrapper, ITypeInfo
    {
        private readonly FixtureServiceProviderMap globalServiceProviders = new();
        private readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders = new();
        private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks = new();

        private readonly ITypeInfo originalTypeInfo;
        private readonly Type originalType;
        private readonly Type wrappedType;
        private readonly object?[] arguments;

        public TypeWrapperDecorator(ITypeInfo originalTypeInfo, Type type, object?[] arguments, Type[] typeArgs)
            : base(type)
        {
            this.originalTypeInfo = originalTypeInfo;
            this.originalType = type;
            if (type.ContainsGenericParameters && !typeArgs.Any())
            {
                arguments = arguments
                    .SkipWhile(arg => arg?.GetType().IsAssignableTo(typeof(Type)) == true)
                    .ToArray();
            }

            this.arguments = arguments;
            this.wrappedType = new LimitingConstructorParametersTypeDecorator(type, arguments);
        }

        Type ITypeInfo.Type => this.wrappedType;

        IMethodInfo[] ITypeInfo.GetMethods(BindingFlags flags) => this
            .GetMethods(flags)
            .Select(method => new PatchAttributesMethodWrapper(method, this.frameworks))
            .ToArray<IMethodInfo>();

        object ITypeInfo.Construct(object?[]? args)
        {
            FixtureAccessor fixtureAccessor = new();

            var currentTest = TestExecutionContext.CurrentContext.CurrentTest;

            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddSingleton(fixtureAccessor)
                .AddSingleton(new FixtureFactoryArgs(this.originalType, args ?? Array.Empty<object>()))
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureFactory>()
                .AddSingleton<IFixtureProvider>(fixtureAccessor)
                .AddSingleton<ITest>(currentTest)
                .AddFixtures(currentTest)
                .AddSingleton(this.globalServiceProviders)
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureServiceProviderSaver>()
                .AddSingleton(this.scopedServiceProviders)
                .AddScoped<IFixtureSetUpService, ScopeServiceProviderSaver>()
                .AddFixtureServiceCollectionModuleAttributes(this.originalTypeInfo));

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

        ITypeInfo ITypeInfo.MakeGenericType(Type[] typeArgs) =>
            new TypeWrapperDecorator(
                this.originalTypeInfo,
                this.originalType.MakeGenericType(typeArgs),
                this.arguments,
                Array.Empty<Type>());

        IMethodInfo[] ITypeInfo.GetMethodsWithAttribute<T>(bool inherit)
            where T : class
        {
            var result = this.GetMethodsWithAttribute<T>(inherit);

            HashSet<Type> fixtureMethods = new()
            {
                typeof(OneTimeSetUpAttribute),
                typeof(OneTimeTearDownAttribute),
            };
            if (fixtureMethods.Contains(typeof(T)))
            {
                result = result
                    .Select(method => new FixtureOneTimeActionMethodInfo(this.wrappedType, method.MethodInfo, this.globalServiceProviders))
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
                    .Select(method => new FixtureActionMethodInfo(this.originalType, method.MethodInfo, this.scopedServiceProviders))
                    .ToArray<IMethodInfo>();
            }

            return result;
        }

        public Task TearDownMethodExample() => Task.CompletedTask;

        private static FixtureServiceProviderFramework GetFramework(ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks, object fixture) =>
            frameworks.GetValue(
                fixture,
                _ => throw new InvalidOperationException($"Service provider framework for {fixture} fixture is not found"));

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

        private static object[] GetServiceArray(IServiceProvider serviceProvider, IEnumerable<ParameterInfo> methodInfo) =>
            methodInfo
                .Select(p => p.ParameterType)
                .Select(type => serviceProvider.GetRequiredService(type))
                .ToArray();

        private record FixtureFactoryArgs(Type FixtureType, object?[] Args);

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
            private readonly IServiceProvider serviceProvider;
            private readonly IFixtureProvider fixtureProvider;
            private readonly FixtureServiceProviderMap map;

            [UsedImplicitly]
            public ScopeServiceProviderSaver(
                ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders,
                IServiceProvider serviceProvider,
                IFixtureProvider fixtureProvider,
                TestAccessor testAccessor)
            {
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
                this.map = scopedServiceProviders.GetOrCreateValue(testAccessor.Test);
            }

            public void Run()
            {
                this.map.Add(this.fixtureProvider.Fixture, this.serviceProvider);
            }

            public void Dispose() => this.map.Remove(this.fixtureProvider.Fixture);
        }

        private class FixtureActionMethodInfo : MethodWrapper, IMethodInfo
        {
            private readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders;

            public FixtureActionMethodInfo(Type type, MethodInfo methodInfo, ConditionalWeakTable<ITest, FixtureServiceProviderMap> scopedServiceProviders)
                : base(type, methodInfo) =>
                this.scopedServiceProviders = scopedServiceProviders;

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var map = this.scopedServiceProviders.GetOrCreateValue(TestExecutionContext.CurrentContext.CurrentTest);
                var serviceProvider = map.Get(fixture ?? throw new InvalidOperationException("Fixture is found for fixture method"));
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

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var testFixture = fixture ?? throw new InvalidOperationException("Fixture is not passed to container dispose method");
                var serviceProvider = this.frameworks.GetValue(
                        testFixture,
                        _ => throw new InvalidOperationException($"Service provider for {fixture} fixture is not found"));

                this.frameworks.Remove(testFixture);
                return serviceProvider.DisposeAsync().AsTask();
            }
        }

        private class FixtureAccessor : IFixtureProvider
        {
            private object? fixture;

            public object Fixture
            {
                get => this.fixture ?? throw new InvalidOperationException("Fixture is not yet set");
                internal set => this.fixture = value;
            }
        }

        private class PatchAttributesMethodWrapper : MethodWrapper, IReflectionInfo
        {
            private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;

            public PatchAttributesMethodWrapper(IMethodInfo methodInfo, ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks)
                : base(methodInfo.TypeInfo.Type, methodInfo.MethodInfo) =>
                this.frameworks = frameworks;

            T[] IReflectionInfo.GetCustomAttributes<T>(bool inherit)
                where T : class
            {
                var customAttributes = this.GetCustomAttributes<T>(inherit);
                if (typeof(T) == typeof(IApplyToContext))
                {
                    return customAttributes.Append((T)(object)new FirstSetUpAction(this.frameworks)).ToArray();
                }

                if (typeof(T) == typeof(IWrapSetUpTearDown))
                {
                    return customAttributes.Append((T)(object)new LastTearDownAction(this.frameworks)).ToArray();
                }

                return typeof(T) == typeof(ITestBuilder)
                    ? customAttributes.Select(ReplaceAttribute).ToArray()
                    : customAttributes;
            }

            private static T ReplaceAttribute<T>(T attribute)
                where T : class =>
                attribute switch
                {
                    TestCaseAttribute testCaseAttribute => (T)(object)new CombinatorialTestCaseAttribute(testCaseAttribute),
                    TestCaseSourceAttribute testCaseSourceAttribute => (T)(object)new CombinatorialTestCaseSourceAttribute(testCaseSourceAttribute),
                    _ => attribute,
                };

            private class FirstSetUpAction : IApplyToContext
            {
                private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;

                public FirstSetUpAction(ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks) =>
                    this.frameworks = frameworks;

                public void ApplyToContext(TestExecutionContext context)
                {
                    var test = context.CurrentTest;
                    if (!test.IsSuite)
                    {
                        GetFramework(this.frameworks, context.TestObject)
                            .RunSetUp(test)
                            .GetAwaiter()
                            .GetResult();
                    }
                }
            }

            private class LastTearDownAction : IWrapSetUpTearDown
            {
                private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;

                public LastTearDownAction(ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks) =>
                    this.frameworks = frameworks;

                public TestCommand Wrap(TestCommand command) => new LastTearDownCommand(command, this.frameworks);

                private class LastTearDownCommand : TestCommand
                {
                    private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;
                    private readonly TestCommand command;

                    public LastTearDownCommand(
                        TestCommand command,
                        ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks)
                        : base(command.Test)
                    {
                        this.command = command;
                        this.frameworks = frameworks;
                    }

                    public override TestResult Execute(TestExecutionContext context)
                    {
                        try
                        {
                            return this.command.Execute(context);
                        }
                        finally
                        {
                            GetFramework(this.frameworks, context.TestObject)
                                .RunTearDown(context.CurrentTest)
                                .GetAwaiter()
                                .GetResult();
                        }
                    }
                }
            }
        }
    }
}