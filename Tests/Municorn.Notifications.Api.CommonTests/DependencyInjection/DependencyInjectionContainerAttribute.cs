using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class DependencyInjectionContainerAttribute : NUnitAttribute, ITestAction
    {
        private readonly ConditionalWeakTable<ITest, ServiceProvider> serviceProviders = new();

        public ActionTargets Targets => ActionTargets.Suite | ActionTargets.Test;

        public void BeforeTest(ITest test)
        {
            if (test.Fixture is not IConfigureServices configureServices)
            {
                throw new InvalidOperationException($"TestFixture {test.Fixture} do not implement {nameof(IConfigureServices)}");
            }

            if (test.IsSuite)
            {
                ServiceCollection serviceCollection = new();
                configureServices.ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true,
                    ValidateScopes = true,
                });
                this.serviceProviders.Add(test, serviceProvider);

                InitializeSingletonFields(configureServices, serviceProvider);
            }
            else
            {
                var testParent = test.Parent!;
                var fixture = testParent is TestFixture testFixture
                    ? testFixture
                    : testParent.Parent!;
                if (!this.serviceProviders.TryGetValue(fixture, out var serviceProvider))
                {
                    throw new InvalidOperationException($"Service provider is not found in fixture {configureServices}");
                }

#pragma warning disable CA2000 // Dispose objects before losing scope
                var serviceScope = serviceProvider.CreateScope();
#pragma warning restore CA2000 // Dispose objects before losing scope
                test.SaveScope(serviceScope);
            }
        }

        public void AfterTest(ITest test)
        {
            if (test.IsSuite)
            {
                if (!this.serviceProviders.TryGetValue(test, out var serviceProvider))
                {
                    throw new InvalidOperationException($"Service provider is not found for test {test}");
                }

                DisposeAsync(serviceProvider).GetAwaiter().GetResult();
                this.serviceProviders.Remove(test);
            }
            else
            {
                test.GetScope().Dispose();
                test.RemoveScope();
            }
        }

        private static async Task DisposeAsync(ServiceProvider serviceProvider)
        {
            await serviceProvider.DisposeAsync().ConfigureAwait(false);
        }

        private static void InitializeSingletonFields(IConfigureServices testFixture, IServiceProvider serviceProvider)
        {
            var fields = testFixture
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttribute<TestDependencyAttribute>() != null);

            foreach (var field in fields)
            {
                var value = serviceProvider.GetRequiredService(field.FieldType);
                field.SetValue(testFixture, value);
            }
        }
    }
}
