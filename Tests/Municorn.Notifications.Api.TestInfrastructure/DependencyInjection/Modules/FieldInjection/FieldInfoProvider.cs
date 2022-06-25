﻿using System.Collections.Generic;
using System.Reflection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [PrimaryConstructor]
    internal partial class FieldInfoProvider
    {
        internal IEnumerable<FieldInfo> Fields { get; }
    }
}