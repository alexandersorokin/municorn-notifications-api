using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields
{
    [PrimaryConstructor]
    internal partial class FieldInfoProvider
    {
        internal IEnumerable<FieldInfo> Fields { get; }
    }
}
