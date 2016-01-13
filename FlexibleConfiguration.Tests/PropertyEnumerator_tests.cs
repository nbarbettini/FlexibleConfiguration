// <copyright file="PropertyEnumerator_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System.Linq;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class PropertyEnumerator_tests
    {
        [Fact]
        public void Enumerates_fully_qualified_members()
        {
            var result = new PropertyEnumerator(typeof(SimpleTestClass))
                .GetProperties();

            result.Count().ShouldBe(3);
            result.ShouldContain("Foo");
            result.ShouldContain("Bar");
            result.ShouldContain("Baz.Quz");
        }

        private class SimpleTestClass
        {
            public string Foo { get; set; }

            public string Bar { get; set; }

            public NestedClass Baz { get; set; }
        }

        private class NestedClass
        {
            public string Quz { get; set; }
        }
    }
}
