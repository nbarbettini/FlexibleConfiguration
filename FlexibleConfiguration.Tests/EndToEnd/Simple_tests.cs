// <copyright file="Simple_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Shouldly;
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

            config.StringProp.ShouldBe(default(string));
            config.IntProp.ShouldBe(default(int));
            config.More.ShouldNotBeNull();
        }

        [Fact]
        public void Adding_empty_string()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();
            configurationBuilder.Add(string.Empty);

            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe(default(string));
            config.IntProp.ShouldBe(default(int));
            config.More.ShouldNotBeNull();
        }

        [Fact]
        public void Adding_values_directly()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("StringProp", "cool");
            configurationBuilder.Add("IntProp", 123);
            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe("cool");
            config.IntProp.ShouldBe(123);
            config.More.ShouldNotBeNull();
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

            config.StringProp.ShouldBe("awesome");
            config.IntProp.ShouldBe(456);
            config.More.Blah.ShouldBe("sweet");
            config.More.Blarg.ShouldBe("rad!");
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

            config.StringProp.ShouldBe("cool");
            config.IntProp.ShouldBe(123);
            config.More.Blah.ShouldBe("foo");
            config.More.Blarg.ShouldBe("bar");
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

            config.StringProp.ShouldBe("awesome");
            config.IntProp.ShouldBe(456);
            config.More.Blah.ShouldBe("sweet");
            config.More.Blarg.ShouldBe("rad!");
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

            config.StringProp.ShouldBe("awesome");
            config.IntProp.ShouldBe(456);
            config.More.Blah.ShouldBe("sweet");
            config.More.Blarg.ShouldBe("rad!");
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

            config.StringProp.ShouldBe("foobar");
            config.More.Blah.ShouldBe("baz");
            config.More.Blarg.ShouldBe("defaults");
        }
    }
}
