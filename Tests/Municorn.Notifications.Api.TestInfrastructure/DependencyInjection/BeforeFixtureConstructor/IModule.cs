using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo);
    }
}
