// <copyright file="Context_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using FlexibleConfiguration.Internal;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class Context_tests
    {
        [Theory]
        [InlineData("foo", "foo")]
        [InlineData("bar", 123)]
        [InlineData("foo.bar", "baz")]
        [InlineData("foo.bar.baz.wheeee", 123)]
        public void Put_and_get(string fullyQualifiedPath, object value)
        {
            var context = new DefaultConfigurationContext();

            context.Put(fullyQualifiedPath, value);

            context.Get(fullyQualifiedPath).ShouldBe(value);
        }

        [Fact]
        public void Getting_nonexistent_value()
        {
            var context = new DefaultConfigurationContext();

            context.Put("foo.bar", null);

            context.Get("foo.bar").ShouldBeNull();
            context.Get("foo.baz").ShouldBeNull();
        }

        [Fact]
        public void Removing_value()
        {
            var context = new DefaultConfigurationContext();

            context.Put("foo.bar.baz", 123);
            context.Remove("foo.bar.baz");

            context.Get("foo.bar.baz").ShouldBe(null);
        }
    }
}
