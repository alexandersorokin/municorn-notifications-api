﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeTestMap.AsyncLocal;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [PrimaryConstructor]
    internal partial class TypeInfoWrapper : ITypeInfo
    {
        private static readonly ConditionalWeakTable<object, ServiceProvider> ServiceProviders = new();

        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ITypeInfo implementation;

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class =>
            this.implementation.GetCustomAttributes<T>(inherit);

        public bool IsDefined<T>(bool inherit)
            where T : class =>
            this.implementation.IsDefined<T>(inherit);

        public bool IsType(Type type) => this.implementation.IsType(type);

        public string GetDisplayName() => this.implementation.GetDisplayName();

        public string GetDisplayName(object?[]? args) => this.implementation.GetDisplayName(args);

        public Type GetGenericTypeDefinition() => this.implementation.GetGenericTypeDefinition();

        public ITypeInfo MakeGenericType(Type[] typeArgs) => this.implementation.MakeGenericType(typeArgs);

        public bool HasMethodWithAttribute(Type attrType) => this.implementation.HasMethodWithAttribute(attrType);

        public IMethodInfo[] GetMethods(BindingFlags flags) => this.implementation
            .GetMethods(flags)
            .Select(method => new ReplaceTestBuilderMethodWrapper(method))
            .ToArray<IMethodInfo>();

        public ConstructorInfo? GetConstructor(Type[] argTypes) => this.implementation.GetConstructor(argTypes);

        public bool HasConstructor(Type[] argTypes) => this.implementation.HasConstructor(argTypes);

        public object Construct(object?[]? args)
        {
            FixtureProvider fixtureProvider = new();

            var serviceCollection = new ServiceCollection()
                .AddSingleton<IFixtureProvider>(fixtureProvider)
                .AddSingleton<AsyncLocalTestCaseServiceResolver>()
                .AddSingleton(typeof(AsyncLocalTestCaseServiceResolver<>))
                .RegisterFixtures(TestExecutionContext.CurrentContext.CurrentTest)
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

            fixtureProvider.Fixture = fixture;
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

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                var customAttributes = this.implementation.GetCustomAttributes(attributeType, inherit);
                if (attributeType == typeof(ITestAction))
                {
                    return customAttributes
                        .Cast<ITestAction>()
                        .Prepend(new DependencyInjectionContainer2Attribute())
                        .ToArray();
                }

                return customAttributes;
            }

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
            private readonly ConcurrentDictionary<ITest, TestData> scopes = new();
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

                this.serviceProvider = ServiceProviders.GetValue(
                    notNullFixture,
                    _ => throw new InvalidOperationException("Fixture is not found"));
                this.fixture = notNullFixture;
            }

            private void BeforeTestCase(ITest test)
            {
                var sp = this.GetServiceProvider(test);
                var serviceScope = sp.CreateAsyncScope();

                var testMethod = (TestMethod)test;
                var originalMethodInfo = testMethod.Method;
                var map = test.GetFixtureServiceProviderMap();
                if (!this.scopes.TryAdd(test, new(serviceScope, originalMethodInfo, map)))
                {
                    throw new InvalidOperationException($"Failed to save original MethodInfo for {test.FullName}");
                }

                var ownFixture = this.GetFixture(test);
                testMethod.Method = new UseContainerMethodInfo(originalMethodInfo, serviceScope.ServiceProvider, ownFixture);

                map.AddScope(ownFixture, serviceScope.ServiceProvider);
            }

            private void AfterTestCase(ITest test)
            {
                if (!this.scopes.TryRemove(test, out var testData))
                {
                    throw new InvalidOperationException($"Failed to get saved TestData for {test.FullName}");
                }

                ((TestMethod)test).Method = testData.OriginalMethodInfo;
                testData.Scope.DisposeSynchronously();

                testData.Map.RemoveScope(this.GetFixture(test));
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

            [MemberNotNull(nameof(fixture))]
            private object GetFixture(ITest test) => this.fixture ?? throw new InvalidOperationException($"Service provider is not initialized for {test.FullName}");

            private record TestData(AsyncServiceScope Scope, IMethodInfo OriginalMethodInfo, FixtureServiceProviderMap Map);
        }

        private class ReplaceTestBuilderMethodWrapper : IMethodInfo
        {
            private readonly IMethodInfo implementation;

            public ReplaceTestBuilderMethodWrapper(IMethodInfo implementation)
            {
                this.implementation = implementation;
            }

            public T[] GetCustomAttributes<T>(bool inherit)
                where T : class
            {
                var result = this.implementation.GetCustomAttributes<T>(inherit);
                if (typeof(T) == typeof(ITestBuilder))
                {
                    return result
                        .Select(attribute => attribute is TestCaseAttribute testCaseAttribute
                            ? (T)(object)new CombinatorialTestCaseAttribute(testCaseAttribute.Arguments)
                            : attribute)
                        .ToArray();
                }

                return result;
            }

            public bool IsDefined<T>(bool inherit)
                where T : class =>
                this.implementation.IsDefined<T>(inherit);

            public IParameterInfo[] GetParameters() => this.implementation.GetParameters();

            public Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

            public IMethodInfo MakeGenericMethod(params Type[] typeArguments) => this.implementation.MakeGenericMethod(typeArguments);

            public object? Invoke(object? fixture, params object?[]? args) => this.implementation.Invoke(fixture, args);

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
        }

        private class FixtureProvider : IFixtureProvider
        {
            private object? fixture;

            public object Fixture
            {
                get => this.fixture ?? throw new InvalidOperationException("Fixture is not yet set");
                set => this.fixture = value;
            }
        }
    }
}