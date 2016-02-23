// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Providers;

namespace FlexibleConfiguration
{
    public static class IConfigurationBuilderProviderExtensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from environment variables
        /// with a specified prefix.
        /// </summary>
        /// <param name="builder">The <see cref="IIConfigurationBuilder"/> to add to.</param>
        /// <param name="mustStartWith">The prefix that environment variable names must start with.</param>
        /// <param name="separator">The separator character or string between key and value names.</param>
        /// <returns>The <see cref="IIConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(
            this IConfigurationBuilder builder,
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
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="json">The JSON string.</param>
        /// <param name="root">
        /// An optional root name to apply to any configuration values.
        /// For example, if <paramref name="root"/> is <c>foo</c>, and the value <c>bar = baz</c>
        /// is discovered, the actual added value will be <c>foo.bar = baz</c>.
        /// </param>
        public static IConfigurationBuilder AddJson(
            this IConfigurationBuilder builder,
            string json,
            string root = null)
        {
            var provider = new JsonProvider(json, root);
            return builder.Add(provider);
        }

        /// <summary>
        /// Adds configuration values from a JSON file.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
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
        public static IConfigurationBuilder AddJsonFile(
            this IConfigurationBuilder builder,
            string filePath,
            bool optional = false,
            string root = null)
        {
            var json = FileOperations.Load(filePath, optional);
            return builder.AddJson(json, root);
        }

        /// <summary>
        /// Adds configuration values from a YAML string.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="yaml">The YAML string.</param>
        /// <param name="root">
        /// An optional root name to apply to any configuration values.
        /// For example, if <paramref name="root"/> is <c>foo</c>, and the value <c>bar = baz</c>
        /// is discovered, the actual added value will be <c>foo.bar = baz</c>.
        /// </param>
        public static IConfigurationBuilder AddYaml(
            this IConfigurationBuilder builder,
            string yaml,
            string root = null)
        {
            var provider = new YamlProvider(yaml, root);
            return builder.Add(provider);
        }

        /// <summary>
        /// Adds configuration values from a YAML file.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
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
        public static IConfigurationBuilder AddYamlFile(
            this IConfigurationBuilder builder,
            string filePath,
            bool optional = false,
            string root = null)
        {
            var yaml = FileOperations.Load(filePath, optional);
            return builder.AddYaml(yaml, root);
        }

        /// <summary>
        /// Adds configuration values from a string.
        /// </summary>
        /// <remarks>
        /// Each line in the string will be treated as a separate configuration value, with the format:
        /// <c>foo.bar = value</c>
        /// </remarks>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <param name="multiLineConfiguration">The configuration string.</param>
        public static IConfigurationBuilder AddProperties(
            this IConfigurationBuilder builder,
            string multiLineConfiguration,
            string root = null)
        {
            var provider = new PropertiesFileProvider(multiLineConfiguration, root);
            return builder.Add(provider);
        }

        /// <summary>
        /// Adds configuration values from a text file.
        /// </summary>
        /// <remarks>
        /// Each line in the file will be treated as a separate configuration value, with the format:
        /// <c>foo.bar = value</c>
        /// </remarks>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
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
        public static IConfigurationBuilder AddPropertiesFile(
            this IConfigurationBuilder builder,
            string filePath,
            bool optional = false,
            string root = null)
        {
            var contents = FileOperations.Load(filePath, optional);
            return builder.AddProperties(contents, root);
        }

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="IConfigurationBuilder">The <see cref="IIConfigurationBuilder"/> to add to.</param>
        /// <param name="initialData">The data to add to memory configuration provider.</param>
        /// <returns>The <see cref="IIConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryCollection(
            this IConfigurationBuilder builder,
            IEnumerable<KeyValuePair<string, string>> initialData)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(IConfigurationBuilder));
            }

            return builder.Add(new MemoryConfigurationProvider(initialData));
        }

        /// <summary>
        /// Adds the configuration in <paramref name="object"/> to <paramref name="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IIConfigurationBuilder"/> to add to.</param>
        /// <param name="object">The object to examine.</param>
        /// <param name="root">A root element to prepend to any discovered key.</param>
        /// <returns>The <see cref="builder"/>.</returns>
        public static IConfigurationBuilder AddObject(
            this IConfigurationBuilder builder,
            object @object,
            string root = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(IConfigurationBuilder));
            }

            return builder.Add(new ObjectReflectionConfigurationProvider(@object, root));
        }
    }
}
