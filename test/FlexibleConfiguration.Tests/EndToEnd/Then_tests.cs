// <copyright file="Then_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Then_tests
    {
        [Fact]
        public void Adding_value_conditionally()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("more.blah", "foobar");
            configurationBuilder.Then(ctx =>
            {
                if (ctx.Exists("more.blah"))
                {
                    ctx.Put("intprop", 123);
                }
            });

            var config = configurationBuilder.Build();

            config.More.Blah.Should().Be("foobar");
            config.IntProp.Should().Be(123);
        }

        [Fact]
        public void Removing_value_conditionally()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

            configurationBuilder.Add("more.blah", "foobar");
            configurationBuilder.Then(ctx =>
            {
                if (ctx.Exists("more.blah"))
                {
                    ctx.Remove("more.blah");
                }
            });

            var config = configurationBuilder.Build();

            config.More.Blah.Should().BeNull();
        }
    }
}
