using System;
using System.Collections;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class InjectDependencyAttribute : NUnitAttribute, IParameterDataSource
    {
        private readonly Type? type;

        public InjectDependencyAttribute(Type type) => this.type = type;

        public InjectDependencyAttribute()
        {
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            var markerType = typeof(InjectedService<>).MakeGenericType(this.type ?? parameter.ParameterType);
            yield return Activator.CreateInstance(markerType);
        }
    }
}
