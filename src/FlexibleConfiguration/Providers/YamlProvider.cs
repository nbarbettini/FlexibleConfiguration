// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Text;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public class YamlProvider : ConfigurationProvider
    {
        private readonly string yaml;
        private readonly string root;

        public YamlProvider(string yaml, string root)
        {
            this.yaml = yaml;
            this.root = root;
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(this.yaml))
            {
                return;
            }

            var parser = new YamlParser(this.root);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(this.yaml)))
            {
                Data = parser.Parse(stream);
            }
        }
    }
}
