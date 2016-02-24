// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexibleConfiguration.Abstractions
{
    /// <summary>
    /// Utility methods and constants for manipulating Configuration paths
    /// </summary>
    public static class ConfigurationPath
    {
        public static readonly string KeyDelimiter = ":";

        public static string Combine(params string[] pathSegements)
        {
            if (pathSegements == null)
            {
                throw new ArgumentNullException(nameof(pathSegements));
            }
            return string.Join(KeyDelimiter, pathSegements);
        }

        public static string Combine(IEnumerable<string> pathSegements)
        {
            if (pathSegements == null)
            {
                throw new ArgumentNullException(nameof(pathSegements));
            }
            return string.Join(KeyDelimiter, pathSegements.ToArray());
        }

        public static string GetSectionKey(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? path : path.Substring(lastDelimiterIndex + 1);
        }

        public static string GetParentPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var lastDelimiterIndex = path.LastIndexOf(KeyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? null : path.Substring(0, lastDelimiterIndex);
        }
    }
}
