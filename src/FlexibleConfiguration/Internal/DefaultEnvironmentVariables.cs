// <copyright file="DefaultEnvironmentVariables.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;

namespace FlexibleConfiguration.Internal
{
    internal sealed class DefaultEnvironmentVariables : IEnvironmentVariables
    {
        public string ExpandEnvironmentVariables(string name)
            => Environment.ExpandEnvironmentVariables(name);

        public string GetEnvironmentVariable(string variable)
            => Environment.GetEnvironmentVariable(variable);

        public IDictionary GetEnvironmentVariables()
            => Environment.GetEnvironmentVariables();
    }
}
