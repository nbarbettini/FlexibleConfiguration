// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public class EnvironmentVariablesProvider : ConfigurationProvider
    {
        private readonly string mustStartWith;
        private readonly string separator;
        private readonly string root;
        private readonly ILogger logger;

        public EnvironmentVariablesProvider(string mustStartWith, string separator, string root, ILogger logger)
        {
            this.mustStartWith = mustStartWith;
            this.separator = separator;
            this.root = root;
            this.logger = logger;
        }

        public override void Load()
        {
            var data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var enumerator = new EnvironmentVariablesEnumerator(mustStartWith, separator);

            try
            {
                foreach (var item in enumerator.GetItems(Environment.GetEnvironmentVariables()))
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
            catch (Exception ex)
            {
                logger?.Log(new LogEntry(LogLevel.Error, string.Empty, $"{nameof(EnvironmentVariablesProvider)}.Load", ex));
            }
        }
    }
}