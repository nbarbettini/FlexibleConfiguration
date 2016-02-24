// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace FlexibleConfiguration.Providers.YamlVisitors
{
    public class ContextAwareMappingVisitor : ContextAwareVisitor
    {
        public ContextAwareMappingVisitor(Stack<string> context = null)
            : base(context)
        {
        }

        protected override void Visit(YamlMappingNode mapping)
        {
            VisitChildren(mapping);
        }
    }
}
