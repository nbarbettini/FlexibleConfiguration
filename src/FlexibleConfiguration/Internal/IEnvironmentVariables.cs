// <copyright file="IEnvironmentVariables.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;

namespace FlexibleConfiguration.Internal
{
#pragma warning disable SA1600 // Elements must be documented
    public interface IEnvironmentVariables
    {
        string ExpandEnvironmentVariables(string name);

        string GetEnvironmentVariable(string variable);

        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        IDictionary GetEnvironmentVariables();

        IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target);
    }
#pragma warning restore SA1600 // Elements must be documented
}
