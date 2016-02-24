// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.IO;

namespace FlexibleConfiguration.Providers
{
    public static class FileOperations
    {
        public static string Load(string path, bool optional)
        {
            if (!File.Exists(path)
                && optional)
            {
                return null;
            }

            return File.ReadAllText(path);
        }
    }
}
