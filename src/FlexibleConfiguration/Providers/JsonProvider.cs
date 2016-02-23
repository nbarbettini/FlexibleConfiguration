// <copyright file="JsonProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;
using YamlDotNet.Serialization;

namespace FlexibleConfiguration.Providers
{
    public sealed class JsonProvider : ConfigurationProvider
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
                var data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var item in enumerator.GetItems())
                {
                    var key = item.Key;

                    if (!string.IsNullOrEmpty(this.root))
                    {
                        key = ConfigurationPath.Combine(this.root, key);
                    }

                    if (data.ContainsKey(key))
                    {
                        throw new FormatException(string.Format($"The key '{key}' is duplicated."));
                    }
                    data[key] = item.Value;
                }

                Data = data;
            }
        }
    }
}
