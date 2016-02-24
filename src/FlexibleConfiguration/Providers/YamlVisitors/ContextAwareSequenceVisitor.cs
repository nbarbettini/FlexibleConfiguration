// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace FlexibleConfiguration.Providers.YamlVisitors
{
    public class ContextAwareSequenceVisitor : ContextAwareVisitor
    {
        private int index = 0;

        public ContextAwareSequenceVisitor(Stack<string> context = null)
            : base(context)
        {
        }

        protected override void Visit(YamlSequenceNode sequence)
        {
            VisitChildren(sequence);
        }

        protected override void VisitChildren(YamlSequenceNode sequence)
        {
            foreach (var node in sequence.Children)
            {
                this.EnterContext(index.ToString());

                var visitor = new ContextAwareVisitor(context);
                node.Accept(visitor);
                this.items.AddRange(visitor.Items);

                this.ExitContext();
                index++;
            }
        }
    }
}
