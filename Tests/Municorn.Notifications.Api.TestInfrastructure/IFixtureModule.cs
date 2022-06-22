using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure
{
    public interface IFixtureModule
    {
        void ConfigureServices(IServiceCollection serviceCollection, ITypeInfo typeInfo);
    }
}
