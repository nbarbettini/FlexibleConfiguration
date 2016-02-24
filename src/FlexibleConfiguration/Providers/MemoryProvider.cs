// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System.Collections;
using System.Collections.Generic;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    public class MemoryConfigurationProvider : ConfigurationProvider, IEnumerable<KeyValuePair<string, string>>
    {
        public MemoryConfigurationProvider()
        {
        }

        public MemoryConfigurationProvider(IEnumerable<KeyValuePair<string, string>> initialData)
        {
            foreach (var pair in initialData)
            {
                Data.Add(pair.Key, pair.Value);
            }
        }

        public void Add(string key, string value)
        {
            Data.Add(key, value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
