using System;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class MethodInfoDecorator : IMethodInfo
    {
        private readonly IMethodInfo methodInfo;

        public T[] GetCustomAttributes<T>(bool inherit)
            where T : class => this.methodInfo.GetCustomAttributes<T>(inherit);

        public bool IsDefined<T>(bool inherit)
            where T : class => this.methodInfo.IsDefined<T>(inherit);

        public IParameterInfo[] GetParameters() => this.methodInfo.GetParameters();

        public Type[] GetGenericArguments() => this.methodInfo.GetGenericArguments();

        public IMethodInfo MakeGenericMethod(params Type[] typeArguments) => this.methodInfo.MakeGenericMethod(typeArguments);

        public object? Invoke(object? fixture, params object?[]? args) => this.methodInfo.Invoke(fixture, args);

        public ITypeInfo TypeInfo => this.methodInfo.TypeInfo;

        public MethodInfo MethodInfo => this.methodInfo.MethodInfo;

        public string Name => this.methodInfo.Name;

        public bool IsAbstract => this.methodInfo.IsAbstract;

        public bool IsPublic => this.methodInfo.IsPublic;

        public bool IsStatic => this.methodInfo.IsStatic;

        public bool ContainsGenericParameters => this.methodInfo.ContainsGenericParameters;

        public bool IsGenericMethod => this.methodInfo.IsGenericMethod;

        public bool IsGenericMethodDefinition => this.methodInfo.IsGenericMethodDefinition;

        public ITypeInfo ReturnType => this.methodInfo.ReturnType;
    }
}
