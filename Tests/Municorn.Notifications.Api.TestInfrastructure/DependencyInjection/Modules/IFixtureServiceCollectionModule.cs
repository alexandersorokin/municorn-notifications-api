using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules
{
    public interface IFixtureServiceCollectionModule
    {
        void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo);
    }
}
