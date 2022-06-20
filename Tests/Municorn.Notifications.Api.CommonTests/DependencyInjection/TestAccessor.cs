using System;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    internal class TestAccessor
    {
        private ITest? test;

        public ITest Test
        {
            get => this.test ?? throw new InvalidOperationException("Test is not yet set");
            set => this.test = value;
        }
    }
}
