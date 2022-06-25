using System;
using System.Threading.Tasks;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    internal class Integration_Test
    {
        private class X : IFixtureSetUpService, IAsyncDisposable
        {
            public void Run()
            {
                throw new NotImplementedException();
            }

            public ValueTask DisposeAsync()
            {
                throw new NotImplementedException();
            }
        }

        private class Y : IFixtureOneTimeSetUpService, IAsyncDisposable
        {
            public void Run()
            {
                throw new NotImplementedException();
            }

            public ValueTask DisposeAsync()
            {
                throw new NotImplementedException();
            }
        }
    }
}
