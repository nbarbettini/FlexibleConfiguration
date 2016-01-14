// <copyright file="AbstractConfigurationProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    internal abstract class AbstractConfigurationProvider
    {
        public void ApplyConfiguration(IConfigurationContext context)
        {
            foreach (var item in this.GetItems())
            {
                context.Put(item.Key, item.Value);
            }
        }

        protected abstract IEnumerable<KeyValuePair<string, object>> GetItems();
    }
}
