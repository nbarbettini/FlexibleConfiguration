// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration.Providers
{
    public class PropertiesFileParser
    {
        private readonly string contents;
        private readonly string root;

        public PropertiesFileParser(string contents, string root)
        {
            this.contents = contents;
            this.root = root;
        }

        public IEnumerable<KeyValuePair<string, string>> GetItems()
        {
            using (var reader = new StringReader(this.contents))
            {
                while (reader.Peek() != -1)
                {
                    var rawLine = reader.ReadLine();

                    // Ignore blank lines
                    if (string.IsNullOrWhiteSpace(rawLine))
                    {
                        continue;
                    }

                    var line = rawLine.Trim();

                    // Ignore comments
                    if (line[0] == '!' || line[0] == '#')
                    {
                        continue;
                    }

                    var key = string.Empty;
                    var value = string.Empty;

                    int separator = line.IndexOf('=');
                    if (separator != -1)
                    {
                        key = line.Substring(0, separator).Trim();
                        value = line.Substring(separator + 1).Trim();
                    }

                    if (key.Contains(ConfigurationPath.KeyDelimiter))
                    {
                        throw new FormatException(string.Format("Unrecognized line format: {0}", rawLine));
                    }

                    key = key.Replace(".", ConfigurationPath.KeyDelimiter);

                    if (!string.IsNullOrEmpty(this.root))
                    {
                        key = ConfigurationPath.Combine(this.root, key);
                    }

                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }
    }
}
