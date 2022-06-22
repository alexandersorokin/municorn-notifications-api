using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.TestActionManagers
{
    public class TestAccessor
    {
        private IServiceProvider? serviceProvider;

        private ITest? test;

        public ITest Test
        {
            get => this.test ?? throw new InvalidOperationException("Test is not yet set");
            internal set => this.test = value;
        }

        public IServiceProvider ServiceProvider
        {
            get => this.serviceProvider ?? throw new InvalidOperationException("ServiceProvider is not yet set");
            internal set => this.serviceProvider = value;
        }
    }
}
