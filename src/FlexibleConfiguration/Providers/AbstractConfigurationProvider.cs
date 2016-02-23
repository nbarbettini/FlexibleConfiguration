// <copyright file="AbstractConfigurationProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace FlexibleConfiguration.Providers
{
    public abstract class AbstractConfigurationProvider
    {
        public void ApplyConfiguration(IConfigurationContext context)
        {
            foreach (var item in this.GetItems())
            {
                if (!item.Equals(default(KeyValuePair<string, string>)))
                {
                    context.Put(item.Key, item.Value);
                }
            }
        }

        protected abstract IEnumerable<KeyValuePair<string, string>> GetItems();
    }
}
