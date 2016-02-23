// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration
{
    /// <summary>
    /// Builds a configuration object from a variety of sources.
    /// </summary>
    public sealed class ConfigurationBuilder
    {
        private readonly IList<IConfigurationProvider> providers = new List<IConfigurationProvider>();

        /// <summary>
        /// Adds a new configuration provider.
        /// </summary>
        /// <param name="provider">The configuration provider to add.</param>
        /// <returns>The same <see cref="IConfigurationBuilder"/>.</returns>
        public ConfigurationBuilder Add(IConfigurationProvider provider)
        {
            provider.Load();
            this.providers.Add(provider);
            return this;
        }

        /// <summary>
        /// Constructs the target configuration object based on the current builder state.
        /// </summary>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public IConfigurationRoot Build()
        {
            return new ConfigurationRoot(this.providers);
        }
    }
}
