using System;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    internal interface IPrimaryConstructorProvider
    {
        PrimaryConstructorParameters? GetPrimaryConstructorParameters(Type type);
    }
}
