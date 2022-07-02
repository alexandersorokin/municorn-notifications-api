using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder.Decorators
{
    [PrimaryConstructor]
    internal partial class ModifyAttributesTestMethodWrapperDecorator : MethodInfoDecorator, IReflectionInfo
    {
        private readonly ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks;

        bool IReflectionInfo.IsDefined<T>(bool inherit) => this.IsDefined<T>(inherit);

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

        private static FixtureServiceProviderFramework GetFramework(ConditionalWeakTable<object, FixtureServiceProviderFramework> frameworks, object fixture) =>
            frameworks.TryGetValue(fixture, out var result)
                ? result
                : throw new InvalidOperationException($"Service provider framework for {fixture} fixture is not found");

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