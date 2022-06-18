using System;
using System.Linq;
using System.Reflection;
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
        private static readonly ServiceProviderOptions Options = new ServiceProviderOptions
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private ServiceProvider? serviceProvider;

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

                this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
                InitializeSingletonFields(configureServices, this.serviceProvider);
            }
            else
            {
                if (this.serviceProvider is null)
                {
                    throw new InvalidOperationException($"Service provider is not found in fixture {configureServices}");
                }

#pragma warning disable CA2000 // Dispose objects before losing scope
                var serviceScope = this.serviceProvider.CreateScope();
#pragma warning restore CA2000 // Dispose objects before losing scope
                test.SaveScope(serviceScope);
            }
        }

        public void AfterTest(ITest test)
        {
            if (test.IsSuite)
            {
                // workaround https://github.com/nunit/nunit/issues/2938
                try
                {
                    if (this.serviceProvider is null)
                    {
                        throw new InvalidOperationException($"Service provider is not found for test {test}");
                    }

                    ConvertValueTaskToTask(this.serviceProvider).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
                }
            }
            else
            {
                test.GetScope().Dispose();
                test.RemoveScope();
            }
        }

        private static async Task ConvertValueTaskToTask(IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
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
