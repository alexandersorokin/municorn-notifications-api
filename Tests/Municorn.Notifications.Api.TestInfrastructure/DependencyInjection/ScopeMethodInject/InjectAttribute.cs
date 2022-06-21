using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class InjectAttribute : NUnitAttribute, IParameterDataSource
    {
        private readonly Type? type;

        public InjectAttribute(Type type) => this.type = type;

        public InjectAttribute()
        {
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            var markerType = typeof(InjectedService<>).MakeGenericType(this.type ?? parameter.ParameterType);
            yield return Activator.CreateInstance(markerType);
        }
    }
}
