// <copyright file="StormpathConfiguration.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

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
