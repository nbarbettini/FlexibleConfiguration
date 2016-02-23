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
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="mustStartWith">The prefix that environment variable names must start with.</param>
        /// <param name="separator">The separator character or string between key and value names.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static ConfigurationBuilder AddEnvironmentVariables(
            this ConfigurationBuilder builder,
            string mustStartWith,
            string separator,
            string root)
        {
            builder.Add(new EnvironmentVariablesProvider(mustStartWith, separator, root));
            return builder;
        }

        /// <summary>
        /// Adds configuration values from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="root">
        /// An optional root name to apply to any configuration values.
        /// For example, if <paramref name="root"/> is <c>foo</c>, and the value <c>bar = baz</c>
        /// is discovered, the actual added value will be <c>foo.bar = baz</c>.
        /// </param>
        public static ConfigurationBuilder AddJson(
            this ConfigurationBuilder builder,
            string json,
            string root = null)
        {
            var provider = new JsonProvider(json, root);
            builder.Add(provider);
            return builder;
        }

        /// <summary>
        /// Adds configuration values from a JSON file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="required">
        /// Determines whether the file can be skipped silently. If <paramref name="required"/> is <see langword="true"/>,
        /// and the file does not exist, a <see cref="System.IO.FileNotFoundException"/> will be thrown. If <paramref name="required"/>
        /// is <see langword="false"/>, the method will return silently.
        /// </param>
        /// <param name="root">
        /// An optional root name to apply to any configuration values.
        /// For example, if <paramref name="root"/> is <c>foo</c>, and the value <c>bar = baz</c>
        /// is discovered, the actual added value will be <c>foo.bar = baz</c>.
        /// </param>
        public static ConfigurationBuilder AddJsonFile(
            this ConfigurationBuilder builder,
            string filePath,
            bool required = true,
            string root = null)
        {
            var json = FileOperations.Load(filePath);
            return builder.AddJson(json, root);
        }
    }
}
