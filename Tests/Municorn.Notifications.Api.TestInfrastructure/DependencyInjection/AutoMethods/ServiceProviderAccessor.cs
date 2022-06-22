using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods
{
    public class ServiceProviderAccessor
    {
        private IServiceProvider? serviceProvider;

        public IServiceProvider ServiceProvider
        {
            get => this.serviceProvider ?? throw new InvalidOperationException("ServiceProvider is not yet set");
            internal set => this.serviceProvider = value;
        }
    }
}
