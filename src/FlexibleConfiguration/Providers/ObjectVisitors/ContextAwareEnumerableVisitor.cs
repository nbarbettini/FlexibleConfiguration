// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FlexibleConfiguration.Providers.ObjectVisitors
{
    public sealed class ContextAwareEnumerableVisitor : ContextAwareObjectVisitor
    {
        private int index = 0;

        public ContextAwareEnumerableVisitor(Stack<string> previousContext)
            : base(previousContext)
        {
        }

        protected override void VisitEnumerable(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return;
            }

            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var typeInfo = enumerator.Current.GetType().GetTypeInfo();

                if (!IsSupportedPrimitive(typeInfo))
                {
                    throw new NotSupportedException($"The type '{typeInfo.Name}' is not supported in an embedded list.");
                }

                this.EnterContext(this.index.ToString());
                VisitPrimitive(enumerator.Current);
                this.ExitContext();

                this.index++;
            }
        }
    }
}
