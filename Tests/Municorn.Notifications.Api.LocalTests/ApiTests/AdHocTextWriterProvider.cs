﻿using System.IO;
using Municorn.Notifications.Api.Tests.Logging;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    [PrimaryConstructor]
    internal partial class AdHocTextWriterProvider : ITextWriterProvider
    {
        private readonly TextWriter textWriter;

        public TextWriter Get() => this.textWriter;
    }
}