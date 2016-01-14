// <copyright file="IConfigurationContext.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

namespace FlexibleConfiguration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="FlexibleConfiguration{T}">configuration builder</see>.
    /// </summary>
    public interface IConfigurationContext
    {
        /// <summary>
        /// Gets the value stored at <paramref name="fullyQualifiedPath"/>.
        /// </summary>
        /// <remarks>If the path cannot be fully traversed, <see langword="null"/> is returned.</remarks>
        /// <param name="fullyQualifiedPath">The fully-qualified path of a configuration value.</param>
        /// <returns>The configuration value, or <see langword="null"/> if no value exists.</returns>
        object Get(string fullyQualifiedPath);

        /// <summary>
        /// Stores a configuration value.
        /// </summary>
        /// <remarks>If a value already exists at <paramref name="fullyQualifiedPath"/>, it is overwritten.</remarks>
        /// <param name="fullyQualifiedPath">The fully-qualified path of the configuration value to add.</param>
        /// <param name="value">The configuration value to store.</param>
        void Put(string fullyQualifiedPath, object value);
    }
}
