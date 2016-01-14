// <copyright file="Simple_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests.EndToEnd
{
    public class Simple_tests
    {
        [Fact]
        public void Building_empty_target()
        {
            var configurationBuilder = new FlexibleConfiguration<TestConfig>();

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
        public void Adding_is_case_insensitive()
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

        private class TestConfig
        {
            public string StringProp { get; set; }

            public int IntProp { get; set; }

            public MoreConfig More { get; set; }
        }

        private class MoreConfig
        {
            public string Blah { get; set; }

            public string Blarg { get; set; }
        }
    }
}
