// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;
using System.Text;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public class JsonProvider : ConfigurationProvider // todo unseal
    {
        private readonly string json;
        private readonly string root;

        public JsonProvider(string json, string root)
        {
            this.json = json;
            this.root = root;
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(this.json))
            {
                return;
            }

            var parser = new YamlParser(this.root);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(this.json)))
            {
                Data = parser.Parse(stream);
            }
        }
    }
}
