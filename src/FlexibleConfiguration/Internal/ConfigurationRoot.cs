// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration.Internal
{
    public class ConfigurationRoot : IConfigurationRoot
    {
        private IList<IConfigurationProvider> _providers;

        public ConfigurationRoot(IList<IConfigurationProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = providers;
        }

        public string this[string key]
        {
            get
            {
                foreach (var provider in _providers.Reverse())
                {
                    string value;

                    if (provider.TryGet(key, out value))
                    {
                        return value;
                    }
                }

                return null;
            }

            set
            {
                if (!_providers.Any())
                {
                    throw new InvalidOperationException("No providers defined.");
                }

                foreach (var provider in _providers)
                {
                    provider.Set(key, value);
                }
            }
        }

        public IEnumerable<IConfigurationSection> GetChildren() => GetChildrenImplementation(null);

        internal IEnumerable<IConfigurationSection> GetChildrenImplementation(string path)
        {
            return _providers
                .Aggregate(Enumerable.Empty<string>(),
                    (seed, source) => source.GetChildKeys(seed, path))
                .Distinct()
                .Select(key => GetSection(path == null ? key : ConfigurationPath.Combine(path, key)));
        }

        public IConfigurationSection GetSection(string key)
        {
            return new ConfigurationSection(this, key);
        }
    }
}
