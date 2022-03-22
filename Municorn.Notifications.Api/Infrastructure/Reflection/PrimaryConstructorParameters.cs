using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    internal record PrimaryConstructorParameters(
        ConstructorInfo ConstructorInfo,
        IReadOnlyList<ParameterInfo> Parameters);
}
