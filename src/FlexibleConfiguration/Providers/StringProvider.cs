// <copyright file="StringProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    public sealed class StringProvider : AbstractConfigurationProvider
    {
        private readonly string config;

        public StringProvider(string config)
        {
            this.config = config;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetItems()
        {
            if (string.IsNullOrEmpty(this.config))
            {
                yield return default(KeyValuePair<string, string>);
            }

            var lines = this.config.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var tokens = line.Split('=');
                if (tokens.Length != 2)
                {
                    throw new ParseException($"Cannot parse configuration line: '{line}'");
                }

                yield return new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim());
            }
        }
    }
}
