using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class SetupFixtureProvider<T>
        where T : class
    {
        private readonly ITest testFixture;

        public T GetSetupFixture()
        {
            var currentTest = this.testFixture;
            while (currentTest != null)
            {
                if (currentTest.Fixture is T fixture)
                {
                    return fixture;
                }

                currentTest = currentTest.Parent;
            }

            throw new ArgumentException($"{typeof(T)} in SetUpFixture not found");
        }
    }
}
