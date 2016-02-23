// <copyright file="Context_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using FlexibleConfiguration.Internal;
using FluentAssertions;
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

            context.Get(fullyQualifiedPath).Should().Be(value?.ToString());
        }

        [Fact]
        public void Getting_nonexistent_value()
        {
            var context = new DefaultConfigurationContext();

            context.Put("foo.bar", null);

            context.Get("foo.bar").Should().BeNull();
            context.Get("foo.baz").Should().BeNull();
        }

        [Fact]
        public void Removing_value()
        {
            var context = new DefaultConfigurationContext();

            context.Put("foo.bar.baz", 123);
            context.Remove("foo.bar.baz");

            context.Get("foo.bar.baz").Should().Be(null);
        }

        [Fact]
        public void Using_custom_separator()
        {
            var context = new DefaultConfigurationContext(":");

            context.Put("foo:bar", "baz");
            context.Put("foo:qux", "123");

            context.Get("foo:bar").Should().Be("baz");
            context.Get("foo:qux").Should().Be("123");
        }
    }
}
