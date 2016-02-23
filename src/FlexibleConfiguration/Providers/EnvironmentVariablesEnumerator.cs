// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration.Providers
{
    public class EnvironmentVariablesEnumerator
    {
        private readonly string prefix;
        private readonly string separator;

        public EnvironmentVariablesEnumerator(string prefix, string separator)
        {
            this.prefix = prefix;
            this.separator = separator;
        }

        public IEnumerable<KeyValuePair<string, string>> GetItems(IDictionary environmentVariables)
        {
            foreach (DictionaryEntry item in environmentVariables)
            {
                var key = item.Key
                    .ToString()
                    .ToLower();

                if (!string.IsNullOrEmpty(this.prefix))
                {
                    if (!key.StartsWith(this.prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // Trim prefix
                    key = key.Substring(this.prefix.Length);

                    // Trim stray beginning separator, if necessary
                    if (key.StartsWith(this.separator, StringComparison.OrdinalIgnoreCase))
                    {
                        key = key.Substring(this.separator.Length);
                    }
                }

                key = key.Replace(separator.ToLower(), ConfigurationPath.KeyDelimiter);

                yield return new KeyValuePair<string, string>(key, item.Value.ToString());
            }
        }
    }
}
