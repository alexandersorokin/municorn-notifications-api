using System;
using System.Collections.Generic;
using System.Linq;

namespace Municorn.Notifications.Api.Infrastructure.Reflection
{
    // Heuristic from https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Mvc/Mvc.Core/src/ModelBinding/Metadata/DefaultBindingMetadataProvider.cs#L108
    [PrimaryConstructor]
    internal partial class PrimaryConstructorProvider : IPrimaryConstructorProvider
    {
        private readonly IPropertiesProvider propertiesProvider;

        public PrimaryConstructorParameters? GetPrimaryConstructorParameters(Type type)
        {
            if (!type.IsRecord())
            {
                return null;
            }

            var constructors = type.GetConstructors();
            if (constructors.Length != 1)
            {
                return null;
            }

            var properties = this.propertiesProvider
                .GetProperties(type)
                .Select(propertyInfo => (propertyInfo.Name, propertyInfo.PropertyType));
            HashSet<(string Name, Type Type)> propertySet = new(properties);

            var constructor = constructors.Single();
            var parameters = constructor.GetParameters();

            var otherSet = parameters.Select(parameterInfo =>
                (parameterInfo.Name ?? throw new InternalServerErrorException($"Parameter {parameterInfo} do not have name"), parameterInfo.ParameterType));
            return propertySet.IsSupersetOf(otherSet)
                ? new(constructor, parameters)
                : null;
        }
    }
}