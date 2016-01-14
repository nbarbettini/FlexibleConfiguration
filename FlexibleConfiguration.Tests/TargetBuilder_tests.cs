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
            fakeContext.Get("Foo").Returns("Working!");
            fakeContext.Get("Bar").Returns(123);

            var builder = new TargetBuilder(typeof(TestFlatObject), fakeContext);
            var result = (TestFlatObject)builder.Build();

            result.Foo.ShouldBe("Working!");
            result.Bar.ShouldBe(123);
        }

        [Fact]
        public void Constructs_nested_object()
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Qux").Returns("Qux");
            fakeContext.Get("Item1.Foo").Returns("Foo1");
            fakeContext.Get("Item1.Bar").Returns(123);
            fakeContext.Get("Item2.Foo").Returns("Foo2");
            fakeContext.Get("Item2.Bar").Returns(456);

            var builder = new TargetBuilder(typeof(TestNestedObject), fakeContext);
            var result = (TestNestedObject)builder.Build();

            result.Qux.ShouldBe("Qux");
            result.Item1.Foo.ShouldBe("Foo1");
            result.Item1.Bar.ShouldBe(123);
            result.Item2.Foo.ShouldBe("Foo2");
            result.Item2.Bar.ShouldBe(456);
        }

        private class TestFlatObject
        {
            public string Foo { get; set; }

            public int Bar { get; set; }
        }

        private class TestNestedObject
        {
            public string Qux { get; set; }

            public TestFlatObject Item1 { get; set; }

            public TestFlatObject Item2 { get; set; }
        }
    }
}
