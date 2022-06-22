using System;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods
{
    internal class ServiceProviderAccessor
    {
        private ServiceProvider? serviceProvider;

        internal ServiceProvider ServiceProvider
        {
            get => this.serviceProvider ?? throw new InvalidOperationException("ServiceProvider is not yet set");
            set => this.serviceProvider = value;
        }
    }
}
