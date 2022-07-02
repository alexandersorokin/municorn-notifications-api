using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [PrimaryConstructor]
    internal sealed partial class UseContainerMethodInfo : IMethodInfo
    {
        private readonly IMethodInfo methodInfo;
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        IParameterInfo[] IMethodInfo.GetParameters() => this.methodInfo.GetParameters();

        Type[] IMethodInfo.GetGenericArguments() => this.methodInfo.GetGenericArguments();

        IMethodInfo IMethodInfo.MakeGenericMethod(params Type[] typeArguments) => this.methodInfo.MakeGenericMethod(typeArguments);

        ITypeInfo IMethodInfo.TypeInfo => this.methodInfo.TypeInfo;

        MethodInfo IMethodInfo.MethodInfo => this.methodInfo.MethodInfo;

        string IMethodInfo.Name => this.methodInfo.Name;

        bool IMethodInfo.IsAbstract => this.methodInfo.IsAbstract;

        bool IMethodInfo.IsPublic => this.methodInfo.IsPublic;

        bool IMethodInfo.IsStatic => this.methodInfo.IsStatic;

        bool IMethodInfo.ContainsGenericParameters => this.methodInfo.ContainsGenericParameters;

        bool IMethodInfo.IsGenericMethod => this.methodInfo.IsGenericMethod;

        bool IMethodInfo.IsGenericMethodDefinition => this.methodInfo.IsGenericMethodDefinition;

        ITypeInfo IMethodInfo.ReturnType => this.methodInfo.ReturnType;

        T[] IReflectionInfo.GetCustomAttributes<T>(bool inherit) => this.methodInfo.GetCustomAttributes<T>(inherit);

        bool IReflectionInfo.IsDefined<T>(bool inherit) => this.methodInfo.IsDefined<T>(inherit);

        object? IMethodInfo.Invoke(object? fixture, params object?[]? args)
        {
            var resolvedArguments = this.ResolveArgs(fixture, args ?? Enumerable.Empty<object?>()).ToArray();
            return this.methodInfo.Invoke(fixture, resolvedArguments);
        }

        private IEnumerable<object?> ResolveArgs(object? methodFixture, IEnumerable<object?> args)
        {
            return
                from arg in args
                let serviceType = (arg as IInjectedService)?.GetServiceType(methodFixture, this.fixtureProvider.Fixture)
                select serviceType is null ? arg : this.serviceProvider.GetRequiredService(serviceType);
        }
    }
}