// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    /// <summary>
    /// A .properties file-based <see cref="ConfigurationProvider"/>.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/.properties"/>
    public class PropertiesFileProvider : ConfigurationProvider
    {
        private readonly string contents;
        private readonly string root;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesFileProvider"/>.
        /// </summary>
        /// <param name="contents">The contents of the .properties configuration file.</param>
        /// <param name="logger">The logger.</param>
        public PropertiesFileProvider(string contents, ILogger logger)
            : this(contents, root: null, logger: logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesFileProvider"/>.
        /// </summary>
        /// <param name="contents">The contents of the .properties configuration file.</param>
        /// <param name="root">A root element to prepend to any discovered key.</param>
        /// <param name="logger">The logger.</param>
        public PropertiesFileProvider(string contents, string root, ILogger logger)
        {
            this.contents = contents;
            this.root = root;
            this.logger = logger;
        }

        public override void Load()
        {
            if (string.IsNullOrEmpty(this.contents))
            {
                return;
            }

            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var parser = new PropertiesFileParser(this.contents, this.root);

            try
            {
                foreach (var pair in parser.GetItems())
                {
                    if (data.ContainsKey(pair.Key))
                    {
                        throw new FormatException($"The key '{pair.Key}' is duplicated.");
                    }

                    data[pair.Key] = pair.Value;
                }

                Data = data;
            }
            catch (Exception ex)
            {
                logger?.Log(new LogEntry(LogLevel.Error, string.Empty, "YamlProvider.Load", ex));
            }
        }
    }
}
