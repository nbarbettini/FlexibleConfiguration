// <copyright file="IEnvironmentVariables.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections;

namespace FlexibleConfiguration.Internal
{
#pragma warning disable SA1600 // Elements must be documented
    public interface IEnvironmentVariables
    {
        string ExpandEnvironmentVariables(string name);

        string GetEnvironmentVariable(string variable);

        IDictionary GetEnvironmentVariables();
    }
#pragma warning restore SA1600 // Elements must be documented
}
