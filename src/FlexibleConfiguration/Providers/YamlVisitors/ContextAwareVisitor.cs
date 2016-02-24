// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleConfiguration.Abstractions;
using YamlDotNet.RepresentationModel;

namespace FlexibleConfiguration.Providers.YamlVisitors
{
    public class ContextAwareVisitor : YamlVisitorBase
    {
        protected readonly Stack<string> context;
        protected readonly List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

        public ContextAwareVisitor(Stack<string> context = null)
        {
            this.context = context == null
                ? new Stack<string>()
                : new Stack<string>(context.Reverse());
        }

        public IReadOnlyList<KeyValuePair<string, string>> Items => this.items;

        protected override void VisitPair(YamlNode key, YamlNode value)
        {
            EnterContext(key as YamlScalarNode);
            value.Accept(this);
            ExitContext();
        }

        protected override void Visit(YamlScalarNode scalar)
        {
            var key = ConfigurationPath.Combine(context.Reverse());

            string value = IsNull(scalar)
                ? string.Empty
                : scalar.Value;

            this.items.Add(new KeyValuePair<string, string>(key, value));
        }

        protected override void Visit(YamlMappingNode mapping)
        {
            var nestedVisitor = new ContextAwareMappingVisitor(context);
            mapping.Accept(nestedVisitor);
            
            foreach (var item in nestedVisitor.Items)
            {
                this.items.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }
        }

        protected override void Visit(YamlSequenceNode sequence)
        {
            var nestedVisitor = new ContextAwareSequenceVisitor(context);
            sequence.Accept(nestedVisitor);

            foreach (var item in nestedVisitor.Items)
            {
                this.items.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }
        }

        protected void EnterContext(YamlScalarNode scalar)
        {
            string value = IsNull(scalar)
                ? string.Empty
                : scalar.Value;

            EnterContext(value);
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

        private static bool IsNull(YamlScalarNode scalar)
        {
            if (string.IsNullOrEmpty(scalar.Value))
            {
                return true;
            }

            if (string.Equals(scalar.Tag, "tag:yaml.org,2002:null", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(scalar.Value, "null", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(scalar.Value, "~", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
