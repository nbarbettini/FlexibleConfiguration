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
            => this.Add(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(fullyQualifiedPath, value) });

        /// <summary>
        /// Adds the given configuration values to the current configuration.
        /// </summary>
        /// <param name="fullyQualifiedItems">
        /// Key/value pairs whose key is a fully-qualified path (like <c>myValue</c> or <c>foo.bar.myValue</c>) and whose value is the configuration value.
        /// </param>
        public void Add(IEnumerable<KeyValuePair<string, object>> fullyQualifiedItems)
        {
            var provider = new ExplicitProvider(fullyQualifiedItems);
            provider.ApplyConfiguration(this.context);
        }

        public void Add(string configLines)
        {
            var provider = new StringProvider(configLines);
            provider.ApplyConfiguration(this.context);
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

        public void AddFile(string filePath, bool required = true, string root = null)
        {
            var contents = ReadFile(filePath, required);
            this.Add(contents);
        }

        public void AddJson(string json, string root = null)
        {
            var provider = new JsonProvider(json, root);
            provider.ApplyConfiguration(this.context);
        }

        public void AddJsonFile(string filePath, bool required = true, string root = null)
        {
            var json = ReadFile(filePath, required);
            this.AddJson(json, root);
        }

        public void AddYaml(string yaml, string root = null)
        {
            var provider = new YamlProvider(yaml, root);
            provider.ApplyConfiguration(this.context);
        }

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
