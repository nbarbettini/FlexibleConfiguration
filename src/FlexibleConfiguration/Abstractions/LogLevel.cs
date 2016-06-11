// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace FlexibleConfiguration.Abstractions
{
    /// <summary>
    /// Represents the log levels handled by <see cref="ILogger"/>.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Trace (lowest) severity level.
        /// </summary>
        Trace,

        /// <summary>
        /// Info severity level.
        /// </summary>
        Info,

        /// <summary>
        /// Warn severity level.
        /// </summary>
        Warn,

        /// <summary>
        /// Error severity level.
        /// </summary>
        Error,

        /// <summary>
        /// Fatal (highest) severity level.
        /// </summary>
        Fatal
    }
}
