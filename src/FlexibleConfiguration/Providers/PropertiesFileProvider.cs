// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesConfigurationProvider"/>.
        /// </summary>
        /// <param name="contents">The contents of the .properties configuration file.</param>
        public PropertiesFileProvider(string contents)
            : this(contents, root: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesConfigurationProvider"/>.
        /// </summary>
        /// <param name="contents">The contents of the .properties configuration file.</param>
        /// <param name="root">A root element to prepend to any discovered key.</param>
        public PropertiesFileProvider(string contents, string root)
        {
            if (string.IsNullOrEmpty(contents))
            {
                throw new ArgumentException("Invalid file path", nameof(contents));
            }

            this.contents = contents;
            this.root = root;
        }

        public override void Load()
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var parser = new PropertiesFileParser(this.contents, this.root);

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
    }
}
