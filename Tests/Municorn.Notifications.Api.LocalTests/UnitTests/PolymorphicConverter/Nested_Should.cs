using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests.PolymorphicConverter
{
    [TestFixture]
    internal class Nested_Should
    {
        [Test]
        public async Task Deserialize()
        {
            Parent data = new()
            {
                Value = new Nested(),
            };
            JsonSerializerOptions options = new()
            {
                Converters =
                {
                    new PolymorphicConverter<IBase>(),
                },
            };
            using MemoryStream stream = new();

            await JsonSerializer.SerializeAsync(stream, data, options).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            var deserialized = await JsonSerializer.DeserializeAsync<Parent>(stream, options).ConfigureAwait(false);

            deserialized.Should().NotBeNull();
            deserialized!.Value.Should().BeOfType<Nested>();
        }

        private class Parent
        {
            public IBase? Value { get; init; }
        }

        internal interface IBase
        {
        }

        [Discriminator("m")]
        private class Nested : IBase
        {
        }
    }
}
