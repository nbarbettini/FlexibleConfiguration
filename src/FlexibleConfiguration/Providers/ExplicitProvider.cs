// <copyright file="ExplicitProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    public sealed class ExplicitProvider : AbstractConfigurationProvider
    {
        private readonly IEnumerable<KeyValuePair<string, string>> items;

        public ExplicitProvider(IEnumerable<KeyValuePair<string, string>> fullyQualifiedItems)
        {
            this.items = fullyQualifiedItems;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetItems()
            => this.items;
    }
}
