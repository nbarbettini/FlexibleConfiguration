// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace FlexibleConfiguration.Abstractions
{
    /// <summary>
    /// A log entry to be passed to a <see cref="ILogger"/>.
    /// </summary>
    public sealed class LogEntry
    {
        /// <summary>
        /// The severity of the event.
        /// </summary>
        public readonly LogLevel Severity;

        /// <summary>
        /// The log message.
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// The source of the event.
        /// </summary>
        public readonly string Source;

        /// <summary>
        /// The exception associated with the event, or <see langword="null"/>.
        /// </summary>
        public readonly Exception Exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry"/> class
        /// with the specified severity, message, source, and exception.
        /// </summary>
        /// <param name="severity">The severity of the event.</param>
        /// <param name="message">The log message.</param>
        /// <param name="source">The source of the event.</param>
        /// <param name="exception">The exception associated with the event, or <see langword="null"/>.</param>
        public LogEntry(LogLevel severity, string message, string source, Exception exception)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (severity < LogLevel.Trace || severity > LogLevel.Fatal)
            {
                throw new ArgumentOutOfRangeException(nameof(severity));
            }

            this.Severity = severity;
            this.Message = message;
            this.Source = source;
            this.Exception = exception;
        }
    }
}
