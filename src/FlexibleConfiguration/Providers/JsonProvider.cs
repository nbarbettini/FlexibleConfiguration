// <copyright file="JsonProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public sealed class JsonProvider : ConfigurationProvider // todo unseal
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
