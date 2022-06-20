using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    [PrimaryConstructor]
    internal partial class UseContainerMethodInfo : IMethodInfo
    {
        private readonly IMethodInfo implementation;
        private readonly IServiceProvider serviceProvider;
        private readonly object ownerFixture;

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class =>
            this.implementation.GetCustomAttributes<T>(inherit);

        public bool IsDefined<T>(bool inherit)
            where T : class =>
            this.implementation.IsDefined<T>(inherit);

        public IParameterInfo[] GetParameters() => this.implementation.GetParameters();

        public Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

        public IMethodInfo MakeGenericMethod(params Type[] typeArguments) => this.implementation.MakeGenericMethod(typeArguments);

        public object? Invoke(object? fixture, params object?[]? args)
        {
            var resolvedArguments = this.ownerFixture == fixture
                ? this.ResolveArgs(args ?? Enumerable.Empty<object?>()).ToArray()
                : args;
            return this.implementation.Invoke(fixture, resolvedArguments);
        }

        private IEnumerable<object?> ResolveArgs(IEnumerable<object?> args)
        {
            return
                from arg in args
                let type = arg?.GetType()
                select type is not null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(InjectedService<>)
                    ? this.serviceProvider.GetRequiredService(type.GetGenericArguments().Single())
                    : arg;
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
    }
}