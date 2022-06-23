using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields
{
    [PrimaryConstructor]
    internal partial class FieldInfoProvider
    {
        internal IEnumerable<FieldInfo> Fields { get; }
    }
}
