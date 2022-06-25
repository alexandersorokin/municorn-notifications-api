using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder
{
    internal sealed class TypeInfoWrapper : TypeWrapper, ITypeInfo
    {
        private static readonly FixtureServiceProviderMap GlobalServiceProviders = new();
        private static readonly ConditionalWeakTable<ITest, FixtureServiceProviderMap> ScopedServiceProviders = new();
        private static readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> Frameworks = new();

        private readonly Type originalType;
        private readonly Type wrappedType;
        private readonly object?[] arguments;

        public TypeInfoWrapper(Type type, object?[] arguments, Type[] typeArgs)
            : base(type)
        {
            this.originalType = type;
            if (type.ContainsGenericParameters && !typeArgs.Any())
            {
                arguments = arguments
                    .SkipWhile(arg => arg?.GetType().IsAssignableTo(typeof(Type)) == true)
                    .ToArray();
            }

            this.arguments = arguments;
            this.wrappedType = new TypeWrapper(type, arguments);
        }

        Type ITypeInfo.Type => this.wrappedType;

        IMethodInfo[] ITypeInfo.GetMethods(BindingFlags flags) => this
            .GetMethods(flags)
            .Select(method => new PatchAttributesMethodWrapper(method))
            .ToArray<IMethodInfo>();

        object ITypeInfo.Construct(object?[]? args)
        {
            FixtureAccessor fixtureAccessor = new();

            var currentTest = TestExecutionContext.CurrentContext.CurrentTest;

#pragma warning disable CA2000 // Dispose objects before losing scope
            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddSingleton(fixtureAccessor)
                .AddSingleton(new FixtureFactoryArgs(this.originalType, args ?? Array.Empty<object>()))
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureFactory>()
                .AddSingleton<IFixtureProvider>(fixtureAccessor)
                .AddSingleton<ITest>(currentTest)
                .AddFixtures(currentTest)
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureServiceProviderSaver>()
                .AddScoped<IFixtureSetUpService, ScopeServiceProviderSaver>()
                .AddFixtureServiceCollectionModuleAttributes(new NUnit.Framework.Internal.TypeWrapper(this.originalType)));
#pragma warning restore CA2000 // Dispose objects before losing scope

            try
            {
                framework.BeforeTestSuite().GetAwaiter().GetResult();

                var fixture = fixtureAccessor.Fixture;
                Frameworks.Add(fixture, framework);
                return fixture;
            }
            catch (Exception)
            {
                framework.DisposeAsync().AsTask().GetAwaiter().GetResult();
                throw;
            }
        }

        ITypeInfo ITypeInfo.MakeGenericType(Type[] typeArgs) =>
            new TypeInfoWrapper(
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
                    .Select(method => new FixtureOneTimeActionMethodInfo(this.wrappedType, method.MethodInfo))
                    .ToArray<IMethodInfo>();

                if (typeof(T) == typeof(OneTimeTearDownAttribute))
                {
                    var tearDownMethodExample = this.GetType().GetMethod(nameof(this.TearDownMethodExample), BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new InvalidOperationException(nameof(this.TearDownMethodExample) + " is not found");
                    result = result.Prepend(new DisposeFixtureMethodInfo(this.wrappedType, tearDownMethodExample)).ToArray();
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
                    .Select(method => new FixtureActionMethodInfo(this.originalType, method.MethodInfo))
                    .ToArray<IMethodInfo>();
            }

            return result;
        }

        public Task TearDownMethodExample() => Task.CompletedTask;

        private static FixtureServiceProviderFramework GetFramework(object fixture) =>
            Frameworks.GetValue(
                fixture,
                _ => throw new InvalidOperationException($"Service provider framework for {fixture} fixture is not found"));

        private static object[] GetServiceArray(IServiceProvider serviceProvider, IEnumerable<ParameterInfo> methodInfo) =>
            methodInfo
                .Select(p => p.ParameterType)
                .Select(type => serviceProvider.GetRequiredService(type))
                .ToArray();

        private class FixtureOneTimeActionMethodInfo : MethodWrapper, IMethodInfo
        {
            public FixtureOneTimeActionMethodInfo(Type type, MethodInfo methodInfo)
                : base(type, methodInfo)
            {
            }

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var serviceProvider = GlobalServiceProviders.Get(fixture ?? throw new InvalidOperationException("Fixture is not passed to OneTime action method"));
                return this.Invoke(fixture, GetServiceArray(serviceProvider, this.MethodInfo.GetParameters()));
            }
        }

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

            [UsedImplicitly]
            public FixtureServiceProviderSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider)
            {
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
            }

            public void Run() => GlobalServiceProviders.Add(this.fixtureProvider.Fixture, this.serviceProvider);

            public void Dispose() => RemoveFixture(this.fixtureProvider.Fixture);

            private static void RemoveFixture(object fixture) => GlobalServiceProviders.Remove(fixture);
        }

        private sealed class ScopeServiceProviderSaver : IFixtureSetUpService, IDisposable
        {
            private readonly IServiceProvider serviceProvider;
            private readonly IFixtureProvider fixtureProvider;
            private readonly FixtureServiceProviderMap map;

            [UsedImplicitly]
            public ScopeServiceProviderSaver(IServiceProvider serviceProvider, IFixtureProvider fixtureProvider, TestAccessor testAccessor)
            {
                this.serviceProvider = serviceProvider;
                this.fixtureProvider = fixtureProvider;
                this.map = ScopedServiceProviders.GetOrCreateValue(testAccessor.Test);
            }

            public void Run()
            {
                this.map.Add(this.fixtureProvider.Fixture, this.serviceProvider);
            }

            public void Dispose() => this.map.Remove(this.fixtureProvider.Fixture);
        }

        private class FixtureActionMethodInfo : MethodWrapper, IMethodInfo
        {
            public FixtureActionMethodInfo(Type type, MethodInfo methodInfo)
                : base(type, methodInfo)
            {
            }

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var map = ScopedServiceProviders.GetOrCreateValue(TestExecutionContext.CurrentContext.CurrentTest);
                var serviceProvider = map.Get(fixture ?? throw new InvalidOperationException("Fixture is found for fixture method"));
                return this.Invoke(fixture, GetServiceArray(serviceProvider, this.MethodInfo.GetParameters()));
            }
        }

        private class DisposeFixtureMethodInfo : MethodWrapper, IMethodInfo
        {
            public DisposeFixtureMethodInfo(Type type, MethodInfo methodInfo)
                : base(type, methodInfo)
            {
            }

            IParameterInfo[] IMethodInfo.GetParameters() => Array.Empty<IParameterInfo>();

            object IMethodInfo.Invoke(object? fixture, params object?[]? args)
            {
                var testFixture = fixture ?? throw new InvalidOperationException("Fixture is not passed to container dispose method");
                var serviceProvider = Frameworks.GetValue(
                        testFixture,
                        _ => throw new InvalidOperationException($"Service provider for {fixture} fixture is not found"));

                Frameworks.Remove(testFixture);
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
            public PatchAttributesMethodWrapper(IMethodInfo methodInfo)
                : base(methodInfo.TypeInfo.Type, methodInfo.MethodInfo)
            {
            }

            T[] IReflectionInfo.GetCustomAttributes<T>(bool inherit)
                where T : class
            {
                var customAttributes = this.GetCustomAttributes<T>(inherit);
                if (typeof(T) == typeof(IApplyToContext))
                {
                    return customAttributes.Append((T)(object)new FirstSetUpAction()).ToArray();
                }

                if (typeof(T) == typeof(IWrapSetUpTearDown))
                {
                    return customAttributes.Append((T)(object)new LastTearDownAction()).ToArray();
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
                public void ApplyToContext(TestExecutionContext context)
                {
                    var test = context.CurrentTest;
                    if (!test.IsSuite)
                    {
                        GetFramework(context.TestObject)
                            .BeforeTestCase(test)
                            .GetAwaiter()
                            .GetResult();
                    }
                }
            }

            private class LastTearDownAction : IWrapSetUpTearDown
            {
                public TestCommand Wrap(TestCommand command) => new LastTearDownCommand(command);

                private class LastTearDownCommand : TestCommand
                {
                    private readonly TestCommand command;

                    public LastTearDownCommand(TestCommand command)
                        : base(command.Test) => this.command = command;

                    public override TestResult Execute(TestExecutionContext context)
                    {
                        try
                        {
                            return this.command.Execute(context);
                        }
                        finally
                        {
                            GetFramework(context.TestObject)
                                .AfterTestCase(context.CurrentTest)
                                .GetAwaiter()
                                .GetResult();
                        }
                    }
                }
            }
        }

        private class TypeWrapper : Type
        {
            private readonly Type implementation;
            private readonly object?[] arguments;

            public TypeWrapper(Type implementation, object?[] arguments)
            {
                this.implementation = implementation;
                this.arguments = arguments;
            }

            public override bool IsDefined(Type attributeType, bool inherit) =>
                this.implementation.IsDefined(attributeType, inherit);

            public override Module Module => this.implementation.Module;

            public override string? Namespace => this.implementation.Namespace;

            public override string Name => this.implementation.Name;

            public override Type? GetElementType() => this.implementation.GetElementType();

            public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) =>
                this.implementation.GetEvent(name, bindingAttr);

            public override EventInfo[] GetEvents(BindingFlags bindingAttr) =>
                this.implementation.GetEvents(bindingAttr);

            public override Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

            public override FieldInfo? GetField(string name, BindingFlags bindingAttr) =>
                this.implementation.GetField(name, bindingAttr);

            public override FieldInfo[] GetFields(BindingFlags bindingAttr) =>
                this.implementation.GetFields(bindingAttr);

            public override MemberInfo[] GetMembers(BindingFlags bindingAttr) =>
                this.implementation.GetMembers(bindingAttr);

            public override MethodInfo[] GetMethods(BindingFlags bindingAttr) =>
                this.implementation.GetMethods(bindingAttr);

            public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) =>
                this.implementation.GetProperties(bindingAttr);

            public override object? InvokeMember(
                string name,
                BindingFlags invokeAttr,
                Binder? binder,
                object? target,
                object?[]? args,
                ParameterModifier[]? modifiers,
                CultureInfo? culture,
                string[]? namedParameters) =>
                this.implementation.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);

            public override Type UnderlyingSystemType => this.implementation.UnderlyingSystemType;

            protected override TypeAttributes GetAttributeFlagsImpl() => this.implementation.Attributes;

            protected override ConstructorInfo? GetConstructorImpl(
                BindingFlags bindingAttr,
                Binder? binder,
                CallingConventions callConvention,
                Type[] types,
                ParameterModifier[]? modifiers) =>
                this.implementation.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);

            protected override MethodInfo? GetMethodImpl(
                string name,
                BindingFlags bindingAttr,
                Binder? binder,
                CallingConventions callConvention,
                Type[]? types,
                ParameterModifier[]? modifiers) =>
                this.implementation.GetMethod(name, bindingAttr, binder, callConvention, types ?? throw new InvalidOperationException("Should not be called"), modifiers);

            protected override bool IsArrayImpl() => this.implementation.IsArray;

            protected override bool IsByRefImpl() => this.implementation.IsByRef;

            protected override bool IsCOMObjectImpl() => this.implementation.IsCOMObject;

            protected override bool IsPointerImpl() => this.implementation.IsPointer;

            protected override bool IsPrimitiveImpl() => this.implementation.IsPrimitive;

            public override Assembly Assembly => this.implementation.Assembly;

            public override string? AssemblyQualifiedName => this.implementation.AssemblyQualifiedName;

            public override Type? BaseType => this.implementation.BaseType;

            public override string? FullName => this.implementation.FullName;

            public override Guid GUID => this.implementation.GUID;

            public override Type? GetNestedType(string name, BindingFlags bindingAttr) =>
                this.implementation.GetNestedType(name, bindingAttr);

            public override Type[] GetNestedTypes(BindingFlags bindingAttr) =>
                this.implementation.GetNestedTypes(bindingAttr);

            public override Type? GetInterface(string name, bool ignoreCase) =>
                this.implementation.GetInterface(name, ignoreCase);

            public override Type[] GetInterfaces() => this.implementation.GetInterfaces();

            public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) =>
                this.implementation.GetConstructors(bindingAttr)
                    .Select(constructorInfo => new ConstructorInfoLimitParameters(constructorInfo, this.arguments))
                    .ToArray<ConstructorInfo>();

            public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
                this.implementation.GetCustomAttributes(attributeType, inherit);

            public override object[] GetCustomAttributes(bool inherit) =>
                this.implementation.GetCustomAttributes(inherit);

            protected override PropertyInfo? GetPropertyImpl(
                string name,
                BindingFlags bindingAttr,
                Binder? binder,
                Type? returnType,
                Type[]? types,
                ParameterModifier[]? modifiers) =>
                this.implementation.GetProperty(name, bindingAttr, binder, returnType, types ?? throw new InvalidOperationException("Should not be called"), modifiers);

            protected override bool HasElementTypeImpl() => this.implementation.HasElementType;

            private class ConstructorInfoLimitParameters : ConstructorInfo
            {
                private readonly ConstructorInfo implementation;
                private readonly object?[] arguments;

                public ConstructorInfoLimitParameters(ConstructorInfo implementation, object?[] arguments)
                {
                    this.implementation = implementation;
                    this.arguments = arguments;
                }

                public override ParameterInfo[] GetParameters() => this.implementation
                    .GetParameters()
                    .Take(this.arguments.Length)
                    .ToArray();

                public override object[] GetCustomAttributes(bool inherit) =>
                    this.implementation.GetCustomAttributes(inherit);

                public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
                    this.implementation.GetCustomAttributes(attributeType, inherit);

                public override bool IsDefined(Type attributeType, bool inherit) =>
                    this.implementation.IsDefined(attributeType, inherit);

                public override Type? DeclaringType => this.implementation.DeclaringType;

                public override string Name => this.implementation.Name;

                public override Type? ReflectedType => this.implementation.ReflectedType;

                public override MethodImplAttributes GetMethodImplementationFlags() =>
                    this.implementation.GetMethodImplementationFlags();

                public override object? Invoke(
                    object? obj,
                    BindingFlags invokeAttr,
                    Binder? binder,
                    object?[]? parameters,
                    CultureInfo? culture) =>
                    this.implementation.Invoke(obj, invokeAttr, binder, parameters, culture);

                public override object Invoke(
                    BindingFlags invokeAttr,
                    Binder? binder,
                    object?[]? parameters,
                    CultureInfo? culture) => this.implementation.Invoke(invokeAttr, binder, parameters, culture);

                public override MethodAttributes Attributes => this.implementation.Attributes;

                public override RuntimeMethodHandle MethodHandle => this.implementation.MethodHandle;
            }
        }
    }
}