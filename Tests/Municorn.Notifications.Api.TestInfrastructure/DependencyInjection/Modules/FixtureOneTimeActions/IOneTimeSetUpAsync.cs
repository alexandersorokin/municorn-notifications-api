using System.Threading.Tasks;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FixtureOneTimeActions
{
    public interface IOneTimeSetUpAsync
    {
        Task OneTimeSetUpAsync();
    }
}
