// <copyright file="ValidationException.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;

namespace FlexibleConfiguration
{
    /// <summary>
    /// Represents a configuration validation error.
    /// </summary>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
