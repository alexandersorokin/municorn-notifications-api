using System.Threading.Tasks;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal interface IClientTopologyFactory
    {
        Task<IClientTopology> GetTopology();
    }
}
