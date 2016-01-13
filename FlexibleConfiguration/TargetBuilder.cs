// <copyright file="TargetBuilder.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleConfiguration
{
    internal sealed class TargetBuilder<T>
        where T : class, new()
    {
        private readonly ConfigurationContext config;

        public TargetBuilder(ConfigurationContext config)
        {
            this.config = config;
        }

        public T Build()
        {
            var instance = new T();

            throw new NotImplementedException();
        }
    }
}
