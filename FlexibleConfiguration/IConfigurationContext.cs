// <copyright file="IConfigurationContext.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleConfiguration
{
    public interface IConfigurationContext
    {
        object Get(string fullyQualifiedPath);
    }
}
