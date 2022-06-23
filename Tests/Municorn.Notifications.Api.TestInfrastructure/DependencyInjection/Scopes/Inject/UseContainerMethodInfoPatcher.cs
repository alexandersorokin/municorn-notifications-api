﻿using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject
{
    internal sealed class UseContainerMethodInfoPatcher : IFixtureSetUp, IDisposable
    {
        private readonly UseContainerMethodInfoFactory useContainerMethodInfoFactory;
        private readonly TestMethod testMethod;
        private readonly IMethodInfo originalMethodInfo;

        public UseContainerMethodInfoPatcher(UseContainerMethodInfoFactory useContainerMethodInfoFactory, TestAccessor testAccessor)
        {
            this.useContainerMethodInfoFactory = useContainerMethodInfoFactory;

            this.testMethod = testAccessor.Test as TestMethod ??
                              throw new InvalidOperationException($"TestCase test should be {nameof(TestMethod)}");
            this.originalMethodInfo = this.testMethod.Method;
        }

        public void Run() => this.testMethod.Method = this.useContainerMethodInfoFactory.Create(this.originalMethodInfo);

        public void Dispose() => this.testMethod.Method = this.originalMethodInfo;
    }
}