// <copyright file="Env_var_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using FlexibleConfiguration.Internal;
using FlexibleConfiguration.Providers;
using FluentAssertions;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class Env_var_tests
    {
        [Fact]
        public void Matching_variables_are_returned()
        {
            var fakeEnvironment = new FakeEnvironmentVariables(new Dictionary<string, object>()
            {
                ["FOO"] = "bar",
                ["BAR"] = 123,
                ["ignored"] = "nope"
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                fullyQualifiedPathsToLookFor: new string[] { "FOO", "BAR" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("foo").Should().Be("bar");
            context.Get("bar").Should().Be("123");
            context.Get("ignored").Should().BeNull();
        }

        [Fact]
        public void Matching_is_case_insensitive()
        {
            var fakeEnvironment = new FakeEnvironmentVariables(new Dictionary<string, object>()
            {
                ["foo"] = "bar",
                ["bAR"] = 123,
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                fullyQualifiedPathsToLookFor: new string[] { "foo", "bar" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("foo").Should().Be("bar");
            context.Get("bar").Should().Be("123");
        }

        [Fact]
        public void Paths_use_underscores()
        {
            var fakeEnvironment = new FakeEnvironmentVariables(new Dictionary<string, object>()
            {
                ["CONFIG_ITEM_ONE"] = "bar",
                ["CONFIG_ITEM_TWO"] = 123,
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                fullyQualifiedPathsToLookFor: new string[] { "config.item.one", "config.item.two" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("config.item.one").Should().Be("bar");
            context.Get("config.item.two").Should().Be("123");
        }

        [Fact]
        public void Matches_specified_prefix()
        {
            var fakeEnvironment = new FakeEnvironmentVariables(new Dictionary<string, object>()
            {
                ["FOO"] = "baz",
                ["TESTING_FOO"] = "qux",
                ["BAR"] = 123,
                ["TESTING_BAR"] = 456
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                fullyQualifiedPathsToLookFor: new string[] { "foo", "bar" },
                prefix: "testing");

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("foo").Should().Be("qux");
            context.Get("bar").Should().Be("456");
        }
    }
}
