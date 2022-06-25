using NSubstitute;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    internal static class SubstituteExtensions
    {
        internal static (TService Service, TInterface Interface) For<TService, TInterface>()
            where TService : class
            where TInterface : class
        {
            var service = Substitute.For<TService, TInterface>();
            return (service, (TInterface)(object)service);
        }

        internal static (TService Service, TInterface1 Interface1, TInterface2 Interface2) For<TService, TInterface1, TInterface2>()
            where TService : class
            where TInterface1 : class
            where TInterface2 : class
        {
            var service = Substitute.For<TService, TInterface1, TInterface2>();
            return (service, (TInterface1)(object)service, (TInterface2)(object)service);
        }
    }
}
