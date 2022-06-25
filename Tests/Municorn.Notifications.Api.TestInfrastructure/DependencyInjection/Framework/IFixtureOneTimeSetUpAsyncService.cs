using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    public interface IFixtureOneTimeSetUpAsyncService
    {
        Task RunAsync();
    }
}
