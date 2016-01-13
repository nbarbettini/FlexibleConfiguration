// <copyright file="IConfigurationContext.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

namespace FlexibleConfiguration
{
    public interface IConfigurationContext
    {
        object Get(string fullyQualifiedPath);

        void Put(string fullyQualifiedPath, object value);
    }
}
