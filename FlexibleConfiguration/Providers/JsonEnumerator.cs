// <copyright file="JsonEnumerator.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace FlexibleConfiguration.Providers
{
    internal sealed class JsonEnumerator
    {
        private readonly JObject deserialized;
        private readonly string root;

        public JsonEnumerator(JObject deserialized, string root)
        {
            this.deserialized = deserialized;
            this.root = root;
        }

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            var result = new List<KeyValuePair<string, object>>();

            foreach (var prop in this.deserialized.Properties())
            {
                var fullyQualifiedName = this.GetFullyQualifiedName(prop.Name);

                if (prop.Type == JTokenType.Object)
                {
                    result.AddRange(
                        new JsonEnumerator((JObject)prop.Value, fullyQualifiedName).GetItems());
                    continue;
                }

                result.Add(new KeyValuePair<string, object>(
                    fullyQualifiedName,
                    prop.Value.ToObject<object>()));
            }

            return result;
        }

        private string GetFullyQualifiedName(string name)
        {
            if (string.IsNullOrEmpty(this.root))
            {
                return name;
            }

            return $"{this.root}.{name}";
        }
    }
}
