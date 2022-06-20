﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal class UseContainerMethodWrapper : IMethodInfo
    {
        private readonly IMethodInfo implementation;
        private readonly ITest test;

        public UseContainerMethodWrapper(IMethodInfo implementation, ITest test)
        {
            this.implementation = implementation;
            this.test = test;
        }

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
            if (fixture is { } methodTarget)
            {
                var arguments = this.ResolveArgs(methodTarget, args ?? Array.Empty<object?>()).ToArray();
                return this.implementation.Invoke(methodTarget, arguments);
            }
            else
            {
                throw new InvalidOperationException("Method is not bound to fixture instance");
            }
        }

        private IEnumerable<object?> ResolveArgs(object fixture, IReadOnlyList<object?> args)
        {
            var serviceProvider = this.test.GetFixtureServiceProviderMap().GetScope(fixture).ServiceProvider;
            var parameters = this.implementation.GetParameters();
            var usedIndex = 0;
            foreach (var parameter in parameters)
            {
                if (parameter.GetCustomAttributes<InjectAttribute>(false).Any())
                {
                    yield return serviceProvider.GetRequiredService(parameter.ParameterType);
                }
                else if (usedIndex < args.Count)
                {
                    yield return args[usedIndex++];
                }
                else if (parameter.IsOptional)
                {
                    yield return Type.Missing;
                }
            }
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