// <copyright file="ExplicitConfigurationProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    internal sealed class ExplicitConfigurationProvider : AbstractConfigurationProvider
    {
        private readonly IEnumerable<KeyValuePair<string, object>> items;

        public ExplicitConfigurationProvider(IEnumerable<KeyValuePair<string, object>> fullyQualifiedItems)
        {
            this.items = fullyQualifiedItems;
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetItems()
            => this.items;
    }
}
