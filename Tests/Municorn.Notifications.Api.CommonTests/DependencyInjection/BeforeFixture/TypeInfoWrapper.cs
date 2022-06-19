using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixture
{
    internal class TypeInfoWrapper : ITypeInfo
    {
        private static readonly ConditionalWeakTable<object, ServiceProvider> ServiceProviders = new();

        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ITypeInfo implementation;

        public TypeInfoWrapper(ITypeInfo implementation) => this.implementation = implementation;

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class
        {
            var customAttributes = this.implementation.GetCustomAttributes<T>(inherit);
            if (typeof(T) == typeof(ITestAction))
            {
                customAttributes = customAttributes
                    .Prepend((T)(object)new DependencyInjectionContainer2Attribute())
                    .ToArray();
            }

            return customAttributes;
        }

        public bool IsDefined<T>(bool inherit)
            where T : class =>
            this.implementation.IsDefined<T>(inherit);

        public bool IsType(Type type) => this.implementation.IsType(type);

        public string GetDisplayName() => this.implementation.GetDisplayName();

        public string GetDisplayName(object?[]? args) => this.implementation.GetDisplayName(args);

        public Type GetGenericTypeDefinition() => this.implementation.GetGenericTypeDefinition();

        public ITypeInfo MakeGenericType(Type[] typeArgs) => this.implementation.MakeGenericType(typeArgs);

        public bool HasMethodWithAttribute(Type attrType) => this.implementation.HasMethodWithAttribute(attrType);

        public IMethodInfo[] GetMethods(BindingFlags flags) => this.implementation.GetMethods(flags);

        public ConstructorInfo? GetConstructor(Type[] argTypes) => this.implementation.GetConstructor(argTypes);

        public bool HasConstructor(Type[] argTypes) => this.implementation.HasConstructor(argTypes);

        public object Construct(object?[]? args)
        {
            var serviceCollection = new ServiceCollection()
                .RegisterWaiter();

            foreach (var module in this.GetCustomAttributes<IModule>(true))
            {
                module.ConfigureServices(serviceCollection);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider(Options);

            var ctorArgs = this.implementation.Type
                .GetConstructors()
                .First()
                .GetParameters()
                .Select(p => p.ParameterType)
                .Select(type => serviceProvider.GetRequiredService(type))
                .ToArray();

            var fixture = this.implementation.Construct(ctorArgs);

            ServiceProviders.Add(fixture, serviceProvider);

            return fixture;
        }

        public IMethodInfo[] GetMethodsWithAttribute<T>(bool inherit)
            where T : class
        {
            var result = this.implementation.GetMethodsWithAttribute<T>(inherit);

            if (typeof(T) == typeof(OneTimeTearDownAttribute))
            {
                return result.ToArray();
            }

            return result;
        }

        public Type Type => new TypeWrapper(this.implementation.Type);

        public ITypeInfo? BaseType => this.implementation.BaseType;

        public string Name => this.implementation.Name;

        public string FullName => this.implementation.FullName;

        public Assembly Assembly => this.implementation.Assembly;

        public string Namespace => this.implementation.Namespace;

        public bool IsAbstract => this.implementation.IsAbstract;

        public bool IsGenericType => this.implementation.IsGenericType;

        public bool ContainsGenericParameters => this.implementation.ContainsGenericParameters;

        public bool IsGenericTypeDefinition => this.implementation.IsGenericTypeDefinition;

        public bool IsSealed => this.implementation.IsSealed;

        public bool IsStaticClass => this.implementation.IsStaticClass;

        internal class TypeWrapper : Type
        {
            private readonly Type implementation;

            public TypeWrapper(Type implementation) => this.implementation = implementation;

            public override object[] GetCustomAttributes(bool inherit) =>
                this.implementation.GetCustomAttributes(inherit);

            public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
                this.implementation.GetCustomAttributes(attributeType, inherit);

            public override bool IsDefined(Type attributeType, bool inherit) =>
                this.implementation.IsDefined(attributeType, inherit);

            public override Module Module => this.implementation.Module;

            public override string? Namespace => this.implementation.Namespace;

            public override string Name => this.implementation.Name;

            public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) =>
                this.implementation.GetConstructors(bindingAttr)
                    .Select(c => new ConstructorInfoWrapper(c))
                    .ToArray<ConstructorInfo>();

            public override Type? GetElementType() => this.implementation.GetElementType();

            public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) =>
                this.implementation.GetEvent(name, bindingAttr);

            public override EventInfo[] GetEvents(BindingFlags bindingAttr) =>
                this.implementation.GetEvents(bindingAttr);

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

            protected override PropertyInfo? GetPropertyImpl(
                string name,
                BindingFlags bindingAttr,
                Binder? binder,
                Type? returnType,
                Type[]? types,
                ParameterModifier[]? modifiers) =>
                this.implementation.GetProperty(name, bindingAttr, binder, returnType, types ?? throw new InvalidOperationException("Should not be called"), modifiers);

            protected override bool HasElementTypeImpl() => this.implementation.HasElementType;

            private class ConstructorInfoWrapper : ConstructorInfo
            {
                private readonly ConstructorInfo implementation;

                public ConstructorInfoWrapper(ConstructorInfo implementation) => this.implementation = implementation;

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

                public override ParameterInfo[] GetParameters() => Array.Empty<ParameterInfo>();

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

        private sealed class DependencyInjectionContainer2Attribute : NUnitAttribute, ITestAction
        {
            private ServiceProvider? serviceProvider;
            private object? fixture;

            public ActionTargets Targets => ActionTargets.Suite | ActionTargets.Test;

            public void BeforeTest(ITest test)
            {
                if (test.IsSuite)
                {
                    this.BeforeTestSuite(test);
                }
                else
                {
                    this.BeforeTestCase(test);
                }
            }

            public void AfterTest(ITest test)
            {
                if (test.IsSuite)
                {
                    this.AfterTestSuite(test);
                }
                else
                {
                    this.AfterTestCase(test);
                }
            }

            private void BeforeTestSuite(ITest test)
            {
                var testFixture = test.Fixture;
                if (testFixture is not { } notNullFixture)
                {
                    throw new InvalidOperationException($"Test {test.FullName} with fixture {testFixture}");
                }

                this.serviceProvider = ServiceProviders.GetValue(notNullFixture, _ => throw new InvalidOperationException("Fixture is not found"));
                this.fixture = notNullFixture;
            }

            private void BeforeTestCase(ITest test)
            {
                var sp = this.GetServiceProvider(test);
                var serviceScope = sp.CreateAsyncScope();
                test.GetFixtureServiceProviderMap().AddScope(this.fixture!, serviceScope);
            }

            private void AfterTestCase(ITest test)
            {
                var map = test.GetFixtureServiceProviderMap();
                map.GetScope(this.fixture!).DisposeSynchronously();
                map.RemoveScope(this.fixture!);
            }

            private void AfterTestSuite(ITest test)
            {
                // workaround https://github.com/nunit/nunit/issues/2938
                try
                {
                    this.GetServiceProvider(test).DisposeSynchronously();
                }
                catch (Exception ex)
                {
                    TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
                }
            }

            [MemberNotNull(nameof(serviceProvider))]
            private ServiceProvider GetServiceProvider(ITest test) => this.serviceProvider ?? throw new InvalidOperationException($"Service provider is not initialized for {test.FullName}");
        }
    }
}