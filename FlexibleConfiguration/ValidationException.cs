// <copyright file="ValidationException.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;

namespace FlexibleConfiguration
{
    /// <summary>
    /// Represents a configuration validation error.
    /// </summary>
    [Serializable]
    public sealed class ValidationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ValidationException(string message)
            : base(message)
        {
        }
    }
}
