using System;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public sealed class InjectFieldDependencyAttribute : Attribute
    {
    }
}
