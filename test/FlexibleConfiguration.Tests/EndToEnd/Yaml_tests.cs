// <copyright file="Yaml_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Yaml_tests
    {
        [Fact]
        public void Parses_flat_values()
        {
            string yaml = @"
stringprop: foobar
intprop: 123
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddYaml(yaml);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("foobar");
            config.IntProp.Should().Be(123);
        }

        [Fact]
        public void Matching_is_case_insensitivez()
        {
            string yaml = @"
STRINGPROP: foobar
Intprop: 123
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddYaml(yaml);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("foobar");
            config.IntProp.Should().Be(123);
        }

        [Fact]
        public void Other_items_are_ignored()
        {
            string yaml = @"
other1: 123
other2: 456
some:
    very:
        important:
            details: foo
stringprop: foobar
intprop: 123
more:
    blarg: foobar
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddYaml(yaml);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("foobar");
            config.IntProp.Should().Be(123);
            config.More.Blarg.Should().Be("foobar");
        }

        [Fact]
        public void Parses_nested_values()
        {
            string yaml = @"
stringprop: yaml ain't markup
intprop: 456
more:
    blah: baz
    blarg: qux
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddYaml(yaml);
            var config = configurationBuilder.Build();

            config.StringProp.Should().Be("yaml ain't markup");
            config.IntProp.Should().Be(456);
            config.More.Blah.Should().Be("baz");
            config.More.Blarg.Should().Be("qux");
        }

        [Fact]
        public void Applies_root()
        {
            string yaml = @"
blah: baz
blarg: qux
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.AddYaml(yaml, "more");
            var config = configurationBuilder.Build();

            config.More.Blah.Should().Be("baz");
            config.More.Blarg.Should().Be("qux");
        }

        [Fact]
        public void Adding_missing_file_throws_when_required()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Action bad = () => configurationBuilder.AddYamlFile("non_existent.yaml", required: true);

            bad.ShouldThrow<FileNotFoundException>();
        }

        [Fact]
        public void Adding_missing_file_returns_null()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Action good = () => configurationBuilder.AddYamlFile("non_existent.yaml", required: false);

            good.ShouldNotThrow();
        }

        [Fact]
        public void Throws_ParseException_for_malformed_yaml()
        {
            string yaml = @"
blah baz
blarg: qux
";
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Action bad = () => configurationBuilder.AddYaml(yaml);

            bad.ShouldThrow<ParseException>();
        }
    }
}
