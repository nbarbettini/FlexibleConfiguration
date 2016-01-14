// <copyright file="YamlEnumerator.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    internal sealed class YamlEnumerator
    {
        private readonly IDictionary<object, object> map;
        private readonly string root;

        public YamlEnumerator(IDictionary<object, object> map, string root)
        {
            this.map = map;
            this.root = root;
        }

        public IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            var result = new List<KeyValuePair<string, object>>();

            foreach (var item in this.map)
            {
                var nestedMap = item.Value as IDictionary<object, object>;
                if (nestedMap != null)
                {
                    result.AddRange(
                        new YamlEnumerator(nestedMap, this.GetFullyQualifiedName(item.Key.ToString()))
                        .GetItems());
                    continue;
                }

                result.Add(new KeyValuePair<string, object>(
                    this.GetFullyQualifiedName(item.Key.ToString()),
                    item.Value));
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
