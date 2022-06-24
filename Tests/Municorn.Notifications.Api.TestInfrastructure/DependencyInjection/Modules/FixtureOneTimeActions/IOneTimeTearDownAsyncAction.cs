using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    public interface IOneTimeTearDownAsyncAction
    {
        Task IOneTimeTearDownAsync();
    }
}
