using System;
using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class InjectParameterDependencyAttribute : NUnitAttribute, IParameterDataSource
    {
        private readonly Type? type;

        public InjectParameterDependencyAttribute(Type type) => this.type = type;

        public InjectParameterDependencyAttribute()
        {
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            var markerType = typeof(InjectedService<>).MakeGenericType(this.type ?? parameter.ParameterType);
            yield return Activator.CreateInstance(markerType);
        }
    }
}
