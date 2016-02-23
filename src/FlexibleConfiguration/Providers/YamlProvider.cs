// <copyright file="YamlProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace FlexibleConfiguration.Providers
{
    internal sealed class YamlProvider : AbstractConfigurationProvider
    {
        private readonly string yaml;
        private readonly string root;

        public YamlProvider(string yaml, string root)
        {
            this.yaml = yaml;
            this.root = root;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetItems()
        {
            if (string.IsNullOrEmpty(this.yaml))
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
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
                return enumerator.GetItems();
            }
        }
    }
}
