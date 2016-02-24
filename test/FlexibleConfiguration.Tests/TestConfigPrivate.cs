// <copyright file="TestConfigPrivate.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

namespace FlexibleConfiguration.Tests
{
    public class TestConfigPrivate
    {
        public string StringProp { get; private set; }

        public int IntProp { get; private set; }

        public MoreConfig More { get; private set; }
    }
}
