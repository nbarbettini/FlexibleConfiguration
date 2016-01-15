// <copyright file="TargetBuilder_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using FlexibleConfiguration.Internal;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class TargetBuilder_tests
    {
        [Fact]
        public void Throws_for_string_target()
        {
            Should.Throw<ArgumentException>(
                () => new TargetBuilder(typeof(string), Substitute.For<IConfigurationContext>()));
        }

        [Fact]
        public void Throws_for_primitive_target()
        {
            Should.Throw<ArgumentException>(
                () => new TargetBuilder(typeof(int), Substitute.For<IConfigurationContext>()));
        }

        [Fact]
        public void Constructs_flat_object()
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Blah").Returns("Working!");
            fakeContext.Get("Blarg").Returns("Ok");

            var builder = new TargetBuilder(typeof(MoreConfig), fakeContext);
            var result = (MoreConfig)builder.Build();

            result.Blah.ShouldBe("Working!");
            result.Blarg.ShouldBe("Ok");
        }

        [Fact]
        public void Constructs_nested_object()
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("StringProp").Returns("Qux");
            fakeContext.Get("IntProp").Returns(123);
            fakeContext.Get("More.Blah").Returns("Foo1");
            fakeContext.Get("More.Blarg").Returns("Foo2");

            var builder = new TargetBuilder(typeof(TestConfig), fakeContext);
            var result = (TestConfig)builder.Build();

            result.StringProp.ShouldBe("Qux");
            result.IntProp.ShouldBe(123);
            result.More.Blah.ShouldBe("Foo1");
            result.More.Blarg.ShouldBe("Foo2");
        }

        [Fact]
        public void Throws_ValidationException_for_mismatched_type()
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("IntProp").Returns("Qux");

            var builder = new TargetBuilder(typeof(TestConfig), fakeContext);

            Should.Throw<ValidationException>(() => builder.Build());
        }

        [Fact]
        public void Constructs_target_with_private_setters()
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("StringProp").Returns("Qux");
            fakeContext.Get("IntProp").Returns(123);
            fakeContext.Get("More.Blah").Returns("Foo1");
            fakeContext.Get("More.Blarg").Returns("Foo2");

            var builder = new TargetBuilder(typeof(TestConfigPrivate), fakeContext);
            var result = (TestConfigPrivate)builder.Build();

            result.StringProp.ShouldBe("Qux");
            result.IntProp.ShouldBe(123);
            result.More.Blah.ShouldBe("Foo1");
            result.More.Blarg.ShouldBe("Foo2");
        }
    }
}
