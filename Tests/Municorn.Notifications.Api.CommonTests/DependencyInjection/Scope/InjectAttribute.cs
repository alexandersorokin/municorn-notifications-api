using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class InjectAttribute : NUnitAttribute, IParameterDataSource
    {
        private readonly Type? type;

        public InjectAttribute(Type? type = null) => this.type = type;

        public IEnumerable GetData(IParameterInfo parameter)
        {
            var markerType = typeof(InjectedService<>).MakeGenericType(this.type ?? parameter.ParameterType);
            yield return Activator.CreateInstance(markerType);
        }
    }
}
