using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal class HideInjectedMethodWrapper : IMethodInfo
    {
        private readonly IMethodInfo implementation;

        public HideInjectedMethodWrapper(IMethodInfo implementation) => this.implementation = implementation;

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class =>
            this.implementation.GetCustomAttributes<T>(inherit);

        public bool IsDefined<T>(bool inherit)
            where T : class =>
            this.implementation.IsDefined<T>(inherit);

        public IParameterInfo[] GetParameters() => this.implementation
            .GetParameters()
            .Where(parameter => !parameter.GetCustomAttributes<InjectAttribute>(false).Any())
            .ToArray();

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
}