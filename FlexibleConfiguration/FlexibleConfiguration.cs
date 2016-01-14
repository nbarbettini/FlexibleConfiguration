// <copyright file="FlexibleConfiguration.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleConfiguration.Internal;
using FlexibleConfiguration.Providers;

namespace FlexibleConfiguration
{
    public class FlexibleConfiguration<T>
        where T : class, new()
    {
        private readonly IConfigurationContext context;

        public FlexibleConfiguration()
        {
            this.context = new DefaultConfigurationContext();
        }

        public void Add(string fullyQualifiedPath, object value)
            => this.Add(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(fullyQualifiedPath, value) });

        public void Add(IEnumerable<KeyValuePair<string, object>> fullyQualifiedItems)
        {
            var provider = new ExplicitConfigurationProvider(fullyQualifiedItems);
            provider.ApplyConfiguration(this.context);
        }

        public T Build()
        {
            var builder = new TargetBuilder(typeof(T), this.context);

            return (T)builder.Build();
        }
    }
}
