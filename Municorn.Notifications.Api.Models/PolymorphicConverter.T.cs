using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Municorn.Notifications.Api
{
    public class PolymorphicConverter<T> : JsonConverter<T>
    {
        private static readonly IReadOnlyDictionary<string, Type> DiscriminatorToTypeMap = typeof(T)
            .Assembly
            .GetTypes()
            .Where(type => typeof(T).IsAssignableFrom(type))
            .SelectMany(type => type
                .GetCustomAttributes<DiscriminatorAttribute>()
                .Select(attribute => (type, attribute)))
            .ToDictionary(pair => pair.attribute.Value, t => t.type);

        private static readonly IReadOnlyDictionary<Type, string> TypeToDiscriminatorMap = DiscriminatorToTypeMap.Keys.ToDictionary(
            discriminator => DiscriminatorToTypeMap[discriminator]);

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(T) == typeToConvert;
        }

        public override void Write(
            Utf8JsonWriter writer,
            T value,
            JsonSerializerOptions options)
        {
            var runtimeType = value?.GetType() ?? throw new JsonException("Null can not be polymorphically serialized");

            writer.WriteStartObject();
            var serialized = JsonSerializer.Serialize(value, runtimeType);
            using (var document = JsonDocument.Parse(serialized))
            {
                writer.WriteString(PolymorphicConverter.PropertyName, TypeToDiscriminatorMap[runtimeType]);
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }

        public override T? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            var readerCopy = reader;
            var discriminatorValue = GetDiscriminatorValue(ref reader);
            if (!DiscriminatorToTypeMap.TryGetValue(discriminatorValue, out var targetType))
            {
                throw new JsonException($"Discriminator value {discriminatorValue} is not supported");
            }

            return (T?)JsonSerializer.Deserialize(ref readerCopy, targetType, options);
        }

        private static string GetDiscriminatorValue(ref Utf8JsonReader reader)
        {
            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            if (!jsonDocument.RootElement.TryGetProperty(PolymorphicConverter.PropertyName, out var discriminator))
            {
                throw new JsonException($"Field {PolymorphicConverter.PropertyName} is missing in object");
            }

            return discriminator.GetString() ?? throw new JsonException($"Field {PolymorphicConverter.PropertyName} value isn't string");
        }
    }
}
