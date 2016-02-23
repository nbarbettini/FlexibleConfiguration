// <copyright file="TargetBuilder_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using FlexibleConfiguration.Internal;
using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class TargetBuilder_tests
    {
        [Fact]
        public void Throws_for_string_target()
        {
            Action bad = () => new TargetBuilder(typeof(string), new DefaultConfigurationContext());

            bad.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Throws_for_primitive_target()
        {
            Action bad = () => new TargetBuilder(typeof(int), new DefaultConfigurationContext());

            bad.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void Constructs_flat_object()
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Blah", "Working!");
            fakeContext.Put("Blarg", "Ok");

            var builder = new TargetBuilder(typeof(MoreConfig), fakeContext);
            var result = (MoreConfig)builder.Build();

            result.Blah.Should().Be("Working!");
            result.Blarg.Should().Be("Ok");
        }

        [Fact]
        public void Constructs_nested_object()
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("StringProp", "Qux");
            fakeContext.Put("IntProp", "123");
            fakeContext.Put("More.Blah", "Foo1");
            fakeContext.Put("More.Blarg", "Foo2");

            var builder = new TargetBuilder(typeof(TestConfig), fakeContext);
            var result = (TestConfig)builder.Build();

            result.StringProp.Should().Be("Qux");
            result.IntProp.Should().Be(123);
            result.More.Blah.Should().Be("Foo1");
            result.More.Blarg.Should().Be("Foo2");
        }

        [Fact]
        public void Throws_ValidationException_for_mismatched_type()
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("IntProp", "Qux");

            var builder = new TargetBuilder(typeof(TestConfig), fakeContext);

            Action bad = () => builder.Build();

            bad.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Constructs_target_with_private_setters()
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("StringProp", "Qux");
            fakeContext.Put("IntProp", "123");
            fakeContext.Put("More.Blah", "Foo1");
            fakeContext.Put("More.Blarg", "Foo2");

            var builder = new TargetBuilder(typeof(TestConfigPrivate), fakeContext);
            var result = (TestConfigPrivate)builder.Build();

            result.StringProp.Should().Be("Qux");
            result.IntProp.Should().Be(123);
            result.More.Blah.Should().Be("Foo1");
            result.More.Blarg.Should().Be("Foo2");
        }
    }
}
