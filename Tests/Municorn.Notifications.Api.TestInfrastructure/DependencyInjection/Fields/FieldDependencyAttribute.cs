using System;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public sealed class FieldDependencyAttribute : Attribute
    {
    }
}
