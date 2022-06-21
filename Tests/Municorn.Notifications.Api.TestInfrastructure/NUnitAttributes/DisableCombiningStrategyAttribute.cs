using System.Collections;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Builders;

namespace Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes
{
    public abstract class DisableCombiningStrategyAttribute : CombiningStrategyAttribute
    {
        private static readonly CombinatorialStrategy CombinatorialStrategy = new();
        private static readonly NothingDataSourceProvider NothingProvider = new();

        protected DisableCombiningStrategyAttribute()
            : base(CombinatorialStrategy, NothingProvider)
        {
        }

        private class NothingDataSourceProvider : IParameterDataProvider
        {
            public bool HasDataFor(IParameterInfo parameter) => true;

            public IEnumerable GetDataFor(IParameterInfo parameter) => Enumerable.Empty<object>();
        }
    }
}