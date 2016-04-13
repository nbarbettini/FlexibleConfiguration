// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlexibleConfiguration.Providers.ObjectVisitors
{
    public sealed class ContextAwareDictionaryVisitor : ContextAwareObjectVisitor
    {
        public ContextAwareDictionaryVisitor(Stack<string> previousContext)
            : base(previousContext)
        {
        }

        protected override void VisitDictionary(IEnumerable dictionary)
        {
            if (dictionary == null)
            {
                return;
            }

            foreach (object entry in dictionary)
            {
                var entryType = entry.GetType();
                var entryTypeInfo = entryType.GetTypeInfo();

                if (!entryTypeInfo.IsGenericType)
                {
                    throw new Exception("Unknown dictionary entry type definition.");
                }

                if (entryType.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
                {
                    throw new Exception("Unknown dictionary entry type.");
                }

                var keyProperty = entryTypeInfo.DeclaredProperties.Where(p => p.Name == "Key").SingleOrDefault();
                var valueProperty = entryTypeInfo.DeclaredProperties.Where(p => p.Name == "Value").SingleOrDefault();

                var key = keyProperty.GetValue(entry);
                var value = valueProperty.GetValue(entry);

                VisitProperty(key.ToString(), value.GetType().GetTypeInfo(), value);
            }
        }
    }
}
