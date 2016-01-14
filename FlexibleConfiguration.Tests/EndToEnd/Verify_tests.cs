// <copyright file="Verify_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Verify_tests
    {
        [Fact]
        public void Throws_if_value_is_missing()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            Should.Throw<ValidationException>(
                () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist"));
        }

        [Fact]
        public void Throws_if_value_is_null()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", null);

            Should.Throw<ValidationException>(
                () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist"));
        }

        [Fact]
        public void Does_not_throw_for_present_value()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "bar");

            Should.NotThrow(
                () => configurationBuilder.Verify(ctx => ctx.Exists("foo"), "Does not exist"));
        }

        [Fact]
        public void Custom_logic_passing_does_not_throw()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "bar");

            Should.NotThrow(
                () => configurationBuilder.Verify(
                    ctx =>
                    {
                        var fooValue = ctx.GetString("foo");
                        return fooValue == "bar";
                    },
                "Failed validation"));
        }

        [Fact]
        public void Custom_logic_failing_throws()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("foo", "baz");

            Should.Throw<ValidationException>(
                () => configurationBuilder.Verify(
                    ctx =>
                {
                    var fooValue = ctx.GetString("foo");
                    return fooValue == "bar";
                },
                "Failed validation"));
        }
    }
}
