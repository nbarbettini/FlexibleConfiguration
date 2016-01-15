// <copyright file="Json_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.IO;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Json_tests
    {
        [Fact]
        public void Parses_flat_values()
        {
            string json = @"
{
    ""stringprop"": ""foobar"",
    ""intprop"": 123
}
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddJson(json);
            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe("foobar");
            config.IntProp.ShouldBe(123);
        }

        [Fact]
        public void Matching_is_case_insensitivez()
        {
            string json = @"
{
    ""STRINGPROP"": ""foobar"",
    ""Intprop"": 123
}
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddJson(json);
            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe("foobar");
            config.IntProp.ShouldBe(123);
        }

        [Fact]
        public void Other_items_are_ignored()
        {
            string json = @"
{
    ""other1"": 123,
    ""other2"": 456,
    ""some"": {
        ""very"": {
            ""important"": {
                ""details"": ""foo""
            }
        }
    },
    ""stringprop"": ""foobar"",
    ""intprop"": 123,
    ""more"": {
        ""blarg"": ""foobar""
    }
}
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddJson(json);
            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe("foobar");
            config.IntProp.ShouldBe(123);
            config.More.Blarg.ShouldBe("foobar");
        }

        [Fact]
        public void Parses_nested_values()
        {
            string json = @"
{
    ""stringprop"": ""javascript object notation"",
    ""intprop"": 456,
    ""more"": {
        ""blah"": ""baz"",
        ""blarg"": ""qux""
    }
}
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddJson(json);
            var config = configurationBuilder.Build();

            config.StringProp.ShouldBe("javascript object notation");
            config.IntProp.ShouldBe(456);
            config.More.Blah.ShouldBe("baz");
            config.More.Blarg.ShouldBe("qux");
        }

        [Fact]
        public void Applies_root()
        {
            string json = @"
{
    ""blah"": ""baz"",
    ""blarg"": ""qux""
}
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddJson(json, "more");
            var config = configurationBuilder.Build();

            config.More.Blah.ShouldBe("baz");
            config.More.Blarg.ShouldBe("qux");
        }

        [Fact]
        public void Adding_missing_file_throws_when_required()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Should.Throw<FileNotFoundException>(() => configurationBuilder.AddJsonFile("non_existent.json", required: true));
        }

        [Fact]
        public void Adding_missing_file_returns_null()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Should.NotThrow(() => configurationBuilder.AddJsonFile("non_existent.json", required: false));
        }
    }
}
