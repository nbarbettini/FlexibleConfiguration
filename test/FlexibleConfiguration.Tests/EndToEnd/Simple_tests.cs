// <copyright file="Simple_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Simple_tests
    {
        [Fact]
        public void Adding_nothing()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            var config = configurationBuilder.Build();

            config.StringProp.Should().Be(default(string));
            config.IntProp.Should().Be(default(int));
            config.More.Should().NotBeNull();
        }

        [Fact]
        public void Adding_empty_string()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();
            configurationBuilder.Add(string.Empty);

            var config = configurationBuilder.Build();

            config.StringProp.Should().Be(default(string));
            config.IntProp.Should().Be(default(int));
            config.More.Should().NotBeNull();
        }

        [Fact]
        public void Adding_values_directly()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("StringProp", "cool");
            configurationBuilder.Add("IntProp", 123);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("cool");
            config.IntProp.Should().Be(123);
            config.More.Should().NotBeNull();
        }

        [Fact]
        public void Adding_nested_values_directly()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("StringProp", "awesome");
            configurationBuilder.Add("IntProp", 456);
            configurationBuilder.Add("More.Blah", "sweet");
            configurationBuilder.Add("More.Blarg", "rad!");
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("awesome");
            config.IntProp.Should().Be(456);
            config.More.Blah.Should().Be("sweet");
            config.More.Blarg.Should().Be("rad!");
        }

        [Fact]
        public void Adding_multiple_values_directly()
        {
            var rawConfig = @"
stringprop = cool
intprop = 123
more.blah = foo
more.blarg = bar
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add(rawConfig);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("cool");
            config.IntProp.Should().Be(123);
            config.More.Blah.Should().Be("foo");
            config.More.Blarg.Should().Be("bar");
        }

        [Fact]
        public void Matching_is_case_insensitive()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("stringprop", "awesome");
            configurationBuilder.Add("Intprop", 456);
            configurationBuilder.Add("MORE.Blah", "sweet");
            configurationBuilder.Add("More.BLARG", "rad!");
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("awesome");
            config.IntProp.Should().Be(456);
            config.More.Blah.Should().Be("sweet");
            config.More.Blarg.Should().Be("rad!");
        }

        [Fact]
        public void Adding_from_list()
        {
            var defaultConfiguration = new Dictionary<string, string>()
            {
                ["stringprop"] = "awesome",
                ["INTPROP"] = "456",
                ["more.blah"] = "sweet",
                ["MORE.Blarg"] = "rad!"
            };

            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add(defaultConfiguration);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("awesome");
            config.IntProp.Should().Be(456);
            config.More.Blah.Should().Be("sweet");
            config.More.Blarg.Should().Be("rad!");
        }

        [Fact]
        public void Newer_values_overwrite_older_values()
        {
            var defaultConfiguration = new Dictionary<string, string>()
            {
                ["stringprop"] = "awesome",
                ["MORE.Blah"] = "sweet",
                ["more.blarg"] = "defaults"
            };

            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add(defaultConfiguration);
            configurationBuilder.Add("STRINGPROP", "foobar");
            configurationBuilder.Add("more.blah", "baz");
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("foobar");
            config.More.Blah.Should().Be("baz");
            config.More.Blarg.Should().Be("defaults");
        }
    }
}
