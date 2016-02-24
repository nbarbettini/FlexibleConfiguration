// <copyright file="Verify_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Verify_tests
    {
        [Fact]
        public void Throws_if_value_is_missing()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Action bad = () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist");

            bad.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Throws_if_value_is_null()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", null);

            Action bad = () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist");

            bad.ShouldThrow<ValidationException>();
        }

        [Fact]
        public void Does_not_throw_for_present_value()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "bar");

            Action good = () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist");

            good.ShouldNotThrow();
        }

        [Fact]
        public void Custom_logic_passing_does_not_throw()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "bar");

            Action good = () => 
                configurationBuilder.Verify(
                    ctx =>
                    {
                        var fooValue = ctx.Get("foo");
                        return fooValue == "bar";
                    },
                "Failed validation");

            good.ShouldNotThrow();
        }

        [Fact]
        public void Custom_logic_failing_throws()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "baz");

            Action bad = () =>
                configurationBuilder.Verify(
                    ctx =>
                    {
                        var fooValue = ctx.Get("foo");
                        return fooValue == "bar";
                    },
                "Failed validation");

            bad.ShouldThrow<ValidationException>();
        }
    }
}
