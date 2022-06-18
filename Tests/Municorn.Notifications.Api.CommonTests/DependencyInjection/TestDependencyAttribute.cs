﻿using System;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    internal sealed class TestDependencyAttribute : Attribute
    {
    }
}
