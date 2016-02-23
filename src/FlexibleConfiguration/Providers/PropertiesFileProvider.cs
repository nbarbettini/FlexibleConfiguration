// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    /// <summary>
    /// A .properties file-based <see cref="ConfigurationProvider"/>.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/.properties"/>
    public class PropertiesConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesConfigurationProvider"/>.
        /// </summary>
        /// <param name="path">Absolute path of the .properties configuration file.</param>
        public PropertiesConfigurationProvider(string path)
            : this(path, optional: false, root: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PropertiesConfigurationProvider"/>.
        /// </summary>
        /// <param name="path">Absolute path of the .properties configuration file.</param>
        /// <param name="optional">Determines if the configuration is optional.</param>
        /// <param name="root">A root element to prepend to any discovered key.</param>
        public PropertiesConfigurationProvider(string path, bool optional, string root)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Invalid file path", nameof(path));
            }

            Optional = optional;
            Path = path;
            Root = root;
        }

        /// <summary>
        /// Gets a value that determines if this instance of <see cref="PropertiesConfigurationProvider"/> is optional.
        /// </summary>
        public bool Optional { get; }

        /// <summary>
        /// The absolute path of the file backing this instance of <see cref="PropertiesConfigurationProvider"/>.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets a root element to prepend to any discovered key.
        /// </summary>
        public string Root { get; }

        /// <summary>
        /// Loads the contents of the file at <see cref="Path"/>.
        /// </summary>
        /// <exception cref="FileNotFoundException">If <see cref="Optional"/> is <c>false</c> and a
        /// file does not exist at <see cref="Path"/>.</exception>
        public override void Load()
        {
            if (!File.Exists(Path))
            {
                if (Optional)
                {
                    Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("File '{0}' not found", Path), Path);
                }
            }
            else
            {
                using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read))
                {
                    Load(stream);
                }
            }
        }

        internal void Load(Stream stream)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var parser = new PropertiesConfigurationFileParser(this.Root);

            foreach (var pair in parser.Parse(stream))
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
