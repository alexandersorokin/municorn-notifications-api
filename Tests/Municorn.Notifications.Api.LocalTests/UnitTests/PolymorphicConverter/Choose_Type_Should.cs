using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.UnitTests.PolymorphicConverter
{
    [TestFixture]
    internal class Choose_Type_Should
    {
        public static readonly IBase[] TypeCases =
        {
            new A(),
            new B(),
        };

        [Test]
        public async Task Deserialize<T>([ValueSource(nameof(TypeCases))] T value)
            where T : IBase
        {
            JsonSerializerOptions options = new()
            {
                Converters =
                {
                    new PolymorphicConverter<IBase>(),
                },
            };
            using MemoryStream stream = new();

            await JsonSerializer.SerializeAsync<IBase>(stream, value, options).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            var deserialized = await JsonSerializer.DeserializeAsync<IBase>(stream, options).ConfigureAwait(false);

            deserialized.Should().BeOfType<T>();
        }

        internal interface IBase
        {
        }

        [Discriminator("a")]
        private class A : IBase
        {
        }

        [Discriminator("b")]
        private class B : IBase
        {
        }
    }
}
