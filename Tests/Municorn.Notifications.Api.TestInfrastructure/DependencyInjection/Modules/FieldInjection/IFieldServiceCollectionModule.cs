using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    public interface IFieldServiceCollectionModule
    {
        void ConfigureServices(IServiceCollection serviceCollection, FieldInfo fieldInfo);
    }
}
