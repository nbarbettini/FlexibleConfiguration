// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;
using FlexibleConfiguration.Providers.ObjectVisitors;

namespace FlexibleConfiguration.Providers
{
    public class ObjectReflectionConfigurationProvider : ConfigurationProvider
    {
        private readonly object sourceObject;
        private readonly string root;
        private readonly ILogger logger;

        public ObjectReflectionConfigurationProvider(object sourceObject, string root, ILogger logger)
        {
            this.sourceObject = sourceObject;
            this.root = root;
            this.logger = logger;
        }

        public override void Load()
        {
            var data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var items = ContextAwareObjectVisitor.Visit(this.sourceObject);

            try
            {
                foreach (var item in items)
                {
                    var key = item.Key;

                    if (!string.IsNullOrEmpty(this.root))
                    {
                        key = ConfigurationPath.Combine(this.root, key);
                    }

                    if (data.ContainsKey(key))
                    {
                        throw new FormatException($"The key '{key}' is duplicated.");
                    }
                    data[key] = item.Value;
                }

                Data = data;
            }
            catch (Exception ex)
            {
                logger?.Log(new LogEntry(LogLevel.Error, string.Empty, $"{nameof(ObjectReflectionConfigurationProvider)}.Load", ex));
            }
        }
    }
}
