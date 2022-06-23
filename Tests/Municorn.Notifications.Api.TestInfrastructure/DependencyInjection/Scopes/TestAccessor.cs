﻿using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    public class TestAccessor
    {
        private ITest? test;

        public ITest Test
        {
            get => this.test ?? throw new InvalidOperationException("Test is not yet set");
            internal set => this.test = value;
        }
    }
}
