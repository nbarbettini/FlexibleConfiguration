// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FlexibleConfiguration.Providers.ObjectVisitors
{
    public sealed class ContextAwareDictionaryVisitor : ContextAwareObjectVisitor
    {
        public ContextAwareDictionaryVisitor(Stack<string> previousContext)
            : base(previousContext)
        {
        }

        protected override void VisitDictionary(IDictionary dictionary)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                VisitProperty(entry.Key.ToString(), entry.Value.GetType().GetTypeInfo(), entry.Value);
            }
        }
    }
}
