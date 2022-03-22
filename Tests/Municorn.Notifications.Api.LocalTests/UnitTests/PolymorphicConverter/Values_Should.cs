using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests.PolymorphicConverter
{
    [TestFixture]
    internal class Values_Should
    {
        [Test]
        public async Task Deserialize()
        {
            Data data = new()
            {
                Value = Guid.NewGuid().ToString(),
            };
            JsonSerializerOptions options = new()
            {
                Converters =
                {
                    new PolymorphicConverter<IBase>(),
                },
            };
            using MemoryStream stream = new();

            await JsonSerializer.SerializeAsync<IBase>(stream, data, options).ConfigureAwait(false);
            Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
            stream.Seek(0, SeekOrigin.Begin);
            var deserialized = await JsonSerializer.DeserializeAsync<IBase>(stream, options).ConfigureAwait(false);

            deserialized.Should()
                .BeOfType<Data>()
                .Which.Value.Should().Be(data.Value);
        }

        internal interface IBase
        {
        }

        [Discriminator("d")]
        private class Data : IBase
        {
            public string? Value { get; init; }
        }
    }
}
