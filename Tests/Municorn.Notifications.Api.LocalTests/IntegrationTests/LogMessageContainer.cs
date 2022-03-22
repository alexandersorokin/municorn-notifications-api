using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Municorn.Notifications.Api.Tests.IntegrationTests
{
    internal class LogMessageContainer
    {
        private readonly ConcurrentBag<string> messages = new();

        internal void Add(string message) => this.messages.Add(message);

        internal IReadOnlyCollection<string> GetMessages() => this.messages;
    }
}
