// <copyright file="FlexibleConfiguration.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

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
            var provider = new ExplicitConfigurationProvider(fullyQualifiedItems);
            provider.ApplyConfiguration(this.context);
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
    }
}
