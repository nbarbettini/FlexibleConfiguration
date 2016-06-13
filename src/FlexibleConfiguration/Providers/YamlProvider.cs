// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Text;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public class YamlProvider : ConfigurationProvider
    {
        private readonly string yaml;
        private readonly string root;
        private readonly ILogger logger;

        public YamlProvider(string yaml, string root, ILogger logger)
        {
            this.yaml = yaml;
            this.root = root;
            this.logger = logger;
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(this.yaml))
            {
                return;
            }

            try
            {
                var parser = new YamlParser(this.root);
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(this.yaml)))
                {
                    Data = parser.Parse(stream);
                }
            }
            catch (YamlDotNet.Core.YamlException ex)
            {
                logger?.Log(new LogEntry(LogLevel.Error, string.Empty, $"{nameof(YamlProvider)}.Load", ex));
            }
        }
    }
}
