// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration.Providers.ObjectVisitors
{
    public class ContextAwareObjectVisitor : ObjectVisitor
    {
        protected readonly Stack<string> context;
        protected readonly List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

        public ContextAwareObjectVisitor(Stack<string> previousContext = null)
        {
            this.context = previousContext == null
                ? new Stack<string>()
                : new Stack<string>(previousContext.Reverse());
        }

        public static IEnumerable<KeyValuePair<string, string>> Visit(object obj)
        {
            var visitor = new ContextAwareObjectVisitor();
            visitor.VisitObject(obj);
            return visitor.items;
        }

        protected override void VisitProperty(string name, TypeInfo propertyTypeInfo, object actualValue)
        {
            EnterContext(name);
            base.VisitProperty(name, propertyTypeInfo, actualValue);
            ExitContext();
        }

        protected override void VisitPrimitive(object primitiveValue)
        {
            if (primitiveValue == null)
            {
                return;
            }

            var key = ConfigurationPath.Combine(context.Reverse());

            this.items.Add(new KeyValuePair<string, string>(key, primitiveValue.ToString() ?? string.Empty));
        }

        protected override void VisitEnumerable(IEnumerable enumerable)
        {
            var visitor = new ContextAwareEnumerableVisitor(this.context);
            visitor.VisitEnumerable(enumerable);
            this.items.AddRange(visitor.items);
        }

        protected override void VisitDictionary(IDictionary dictionary)
        {
            var visitor = new ContextAwareDictionaryVisitor(this.context);
            visitor.VisitDictionary(dictionary);
            this.items.AddRange(visitor.items);
        }

        protected void EnterContext(string context)
        {
            context = context ?? string.Empty;
            this.context.Push(context);
        }

        protected void ExitContext()
        {
            this.context.Pop();
        }
    }
}
