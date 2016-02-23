// <copyright file="FlexibleConfiguration.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Internal;
using FlexibleConfiguration.Providers;

namespace FlexibleConfiguration
{
    /// <summary>
    /// Builds a configuration object from a variety of sources.
    /// </summary>
    /// <typeparam name="T">The .NET type that will store the configuration.</typeparam>
    public class FlexibleConfiguration<T>
        where T : class, new()
    {
        private readonly IConfigurationContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlexibleConfiguration{T}"/> class.
        /// </summary>
        public FlexibleConfiguration()
        {
            this.context = new DefaultConfigurationContext();
        }

        /// <summary>
        /// Adds a configuration value to the current configuration.
        /// </summary>
        /// <param name="fullyQualifiedPath">The fully-qualified path, like <c>myValue</c> or <c>foo.bar.myValue</c>.</param>
        /// <param name="value">The configuration value to store at <paramref name="fullyQualifiedPath"/>.</param>
        public void Add(string fullyQualifiedPath, object value)
            => this.Add(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>(fullyQualifiedPath, value?.ToString()) });

        /// <summary>
        /// Adds the given configuration values to the current configuration.
        /// </summary>
        /// <param name="fullyQualifiedItems">
        /// Key/value pairs whose key is a fully-qualified path (like <c>myValue</c> or <c>foo.bar.myValue</c>) and whose value is the configuration value.
        /// </param>
        public void Add(IEnumerable<KeyValuePair<string, string>> fullyQualifiedItems)
        {
            var provider = new ExplicitProvider(fullyQualifiedItems);
            provider.ApplyConfiguration(this.context);
        }

        /// <summary>
        /// Adds configuration values from a string.
        /// </summary>
        /// <remarks>
        /// Each line in the string will be treated as a separate configuration value, with the format:
        /// <c>foo.bar = value</c>
        /// </remarks>
        /// <param name="multiLineConfiguration">The configuration string.</param>
        public void Add(string multiLineConfiguration)
        {
            var provider = new StringProvider(multiLineConfiguration);
            provider.ApplyConfiguration(this.context);
        }

        /// <summary>
        /// Adds configuration values from a text file.
        /// </summary>
        /// <remarks>
        /// Each line in the file will be treated as a separate configuration value, with the format:
        /// <c>foo.bar = value</c>
        /// </remarks>
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
        public void AddFile(string filePath, bool required = true, string root = null)
        {
            var contents = ReadFile(filePath, required);
            this.Add(contents);
        }

        /// <summary>
        /// Adds configuration from local environment variables.
        /// </summary>
        /// <param name="fullyQualifiedPathsToLookFor">The keys or paths to look for. Paths with periods (like <c>foo.bar</c>) are converted to underscores (like <c>foo_bar</c>).</param>
        /// <param name="prefix">A prefix (like <c>prefix_</c>) to apply to <paramref name="fullyQualifiedPathsToLookFor"/>, or <see langword="null"/>.</param>
        /// <param name="target">The <see cref="EnvironmentVariableTarget"/> location to retrieve environment variables from.</param>
        public void AddEnvironmentVariables(IEnumerable<string> fullyQualifiedPathsToLookFor, string prefix = null, EnvironmentVariableTarget target = default(EnvironmentVariableTarget))
        {
            var provider = new EnvironmentVariablesProvider(new DefaultEnvironmentVariables(), target, fullyQualifiedPathsToLookFor, prefix);
            provider.ApplyConfiguration(this.context);
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
        public void AddJson(string json, string root = null)
        {
            var provider = new JsonProvider(json, root);
            provider.ApplyConfiguration(this.context);
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
        public void AddJsonFile(string filePath, bool required = true, string root = null)
        {
            var json = ReadFile(filePath, required);
            this.AddJson(json, root);
        }

        /// <summary>
        /// Adds configuration values from a YAML string.
        /// </summary>
        /// <param name="yaml">The YAML string.</param>
        /// <param name="root">
        /// An optional root name to apply to any configuration values.
        /// For example, if <paramref name="root"/> is <c>foo</c>, and the value <c>bar = baz</c>
        /// is discovered, the actual added value will be <c>foo.bar = baz</c>.
        /// </param>
        public void AddYaml(string yaml, string root = null)
        {
            var provider = new YamlProvider(yaml, root);
            provider.ApplyConfiguration(this.context);
        }

        /// <summary>
        /// Adds configuration values from a YAML file.
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
        public void AddYamlFile(string filePath, bool required = true, string root = null)
        {
            var yaml = ReadFile(filePath, required);
            this.AddYaml(yaml, root);
        }

        /// <summary>
        /// Determines whether a value exists at the given path.
        /// </summary>
        /// <param name="fullyQualifiedPath">The fully-qualified path, like <c>myValue</c> or <c>foo.bar.myValue</c>.</param>
        /// <returns><see langword="true"/> if the value exists; <see langword="false"/> otherwise.</returns>
        public bool Exists(string fullyQualifiedPath)
            => this.context.Exists(fullyQualifiedPath);

        /// <summary>
        /// Executes an action against the current builder state.
        /// </summary>
        /// <remarks>Use this method to run additional post-processing after loading a configuration.</remarks>
        /// <param name="postProcessingAction">The action to perform.</param>
        public void Then(Action<IConfigurationContext> postProcessingAction)
        {
            postProcessingAction(this.context);
        }

        /// <summary>
        /// Verifies that the specified condition has been met. If not, a <see cref="ValidationException"/> is thrown.
        /// </summary>
        /// <param name="verificationFunc">The validation condition.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        public void Verify(Func<IConfigurationContext, bool> verificationFunc, string errorMessage)
        {
            if (!verificationFunc(this.context))
            {
                throw new ValidationException(errorMessage);
            }
        }

        /// <summary>
        /// Gets a raw value from the internal configuration context.
        /// </summary>
        /// <param name="fullyQualifiedPath">The fully-qualified path, like <c>myValue</c> or <c>foo.bar.myValue</c>.</param>
        /// <returns>The value stored at <paramref name="fullyQualifiedPath"/>, or <see langword="null"/>.</returns>
        public string Get(string fullyQualifiedPath)
            => this.context.Get(fullyQualifiedPath);

        /// <summary>
        /// Constructs the target configuration object based on the current builder state.
        /// </summary>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        public T Build()
        {
            var builder = new TargetBuilder(typeof(T), this.context);

            return (T)builder.Build();
        }

        private static string ReadFile(string filePath, bool required)
        {
            if (!System.IO.File.Exists(filePath)
                && !required)
            {
                return null;
            }

            return System.IO.File.ReadAllText(filePath);
        }
    }
}
