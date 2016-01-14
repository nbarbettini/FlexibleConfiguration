// <copyright file="Env_var_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using FlexibleConfiguration.Internal;
using FlexibleConfiguration.Providers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class Env_var_tests
    {
        [Fact]
        public void Target_argument_is_passed_down()
        {
            var fakeEnvironment = GetMockEnvironment();

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                EnvironmentVariableTarget.Machine,
                fullyQualifiedPathsToLookFor: new string[] { "FOO", "BAR" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            fakeEnvironment
                .Received()
                .GetEnvironmentVariables(EnvironmentVariableTarget.Machine);
        }

        [Fact]
        public void Matching_variables_are_returned()
        {
            var fakeEnvironment = GetMockEnvironment(new Dictionary<string, object>()
            {
                ["FOO"] = "bar",
                ["BAR"] = 123,
                ["ignored"] = "nope"
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                default(EnvironmentVariableTarget),
                fullyQualifiedPathsToLookFor: new string[] { "FOO", "BAR" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("foo").ShouldBe("bar");
            context.Get("bar").ShouldBe(123);
            context.Get("ignored").ShouldBeNull();
        }

        [Fact]
        public void Paths_use_underscores()
        {
            var fakeEnvironment = GetMockEnvironment(new Dictionary<string, object>()
            {
                ["CONFIG_ITEM_ONE"] = "bar",
                ["CONFIG_ITEM_TWO"] = 123,
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                default(EnvironmentVariableTarget),
                fullyQualifiedPathsToLookFor: new string[] { "config.item.one", "config.item.two" },
                prefix: null);

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("config.item.one").ShouldBe("bar");
            context.Get("config.item.two").ShouldBe(123);
        }

        [Fact]
        public void Matches_specified_prefix()
        {
            var fakeEnvironment = GetMockEnvironment(new Dictionary<string, object>()
            {
                ["FOO"] = "baz",
                ["TESTING_FOO"] = "qux",
                ["BAR"] = 123,
                ["TESTING_BAR"] = 456
            });

            var provider = new EnvironmentVariablesProvider(
                fakeEnvironment,
                default(EnvironmentVariableTarget),
                fullyQualifiedPathsToLookFor: new string[] { "foo", "bar" },
                prefix: "testing");

            var context = new DefaultConfigurationContext();
            provider.ApplyConfiguration(context);

            context.Get("foo").ShouldBe("qux");
            context.Get("bar").ShouldBe(456);
        }

        private static IEnvironmentVariables GetMockEnvironment(IDictionary fakeVariables = null)
        {
            if (fakeVariables == null)
            {
                fakeVariables = new Dictionary<string, object>();
            }

            var fakeEnvironment = Substitute.For<IEnvironmentVariables>();
            fakeEnvironment
                .GetEnvironmentVariables(Arg.Any<EnvironmentVariableTarget>())
                .Returns(fakeVariables);

            return fakeEnvironment;
        }
    }
}
