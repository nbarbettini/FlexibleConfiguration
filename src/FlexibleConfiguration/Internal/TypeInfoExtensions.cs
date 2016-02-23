// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reflection;

namespace FlexibleConfiguration.Internal
{
    public static class TypeInfoExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
        {
            while (typeInfo != null)
            {
                foreach (var property in typeInfo.DeclaredProperties)
                {
                    yield return property;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}
