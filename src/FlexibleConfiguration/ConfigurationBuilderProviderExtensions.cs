// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using FlexibleConfiguration.Providers;

namespace FlexibleConfiguration
{
    public static class ConfigurationBuilderProviderExtensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables
        /// with a specified prefix.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="mustStartWith">The prefix that environment variable names must start with.</param>
        /// <param name="separator">The separator character or string between key and value names.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static ConfigurationBuilder AddEnvironmentVariables(
            this ConfigurationBuilder configurationBuilder,
            string mustStartWith,
            string separator,
            string root)
        {
            configurationBuilder.Add(new EnvironmentVariablesProvider(mustStartWith, separator, root));
            return configurationBuilder;
        }
    }
}
