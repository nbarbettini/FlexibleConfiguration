// <copyright file="JsonProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace FlexibleConfiguration.Providers
{
    internal sealed class JsonProvider : AbstractConfigurationProvider
    {
        private readonly string json;
        private readonly string root;

        public JsonProvider(string json, string root)
        {
            this.json = json;
            this.root = root;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetItems()
        {
            if (string.IsNullOrEmpty(this.json))
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }

            var deserializer = new Deserializer();
            IDictionary<object, object> deserialized = null;

            using (var reader = new StringReader(this.json))
            {
                try
                {
                    deserialized = deserializer.Deserialize(reader) as IDictionary<object, object>;
                }
                catch (Exception ex)
                {
                    throw new ParseException("Error parsing JSON. See the inner exception for details.", ex);
                }

                if (deserialized == null)
                {
                    throw new ParseException("Error parsing JSON. The result is null.");
                }

                var enumerator = new YamlEnumerator(deserialized, this.root);
                return enumerator.GetItems();
            }
        }
    }
}
