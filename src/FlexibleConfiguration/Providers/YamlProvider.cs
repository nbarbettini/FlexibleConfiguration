// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;
using YamlDotNet.Serialization;

namespace FlexibleConfiguration.Providers
{
    public sealed class YamlProvider : ConfigurationProvider
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

            var deserializer = new Deserializer();
            IDictionary<object, object> deserialized = null;

            using (var reader = new StringReader(this.yaml))
            {
                try
                {
                    deserialized = deserializer.Deserialize(reader) as IDictionary<object, object>;
                }
                catch (Exception ex)
                {
                    throw new ParseException("Error parsing YAML. See the inner exception for details.", ex);
                }

                if (deserialized == null)
                {
                    throw new ParseException("Error parsing YAML. The result is null.");
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
