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

        [Theory]
        [InlineData(123)]
        [InlineData("123")]
        [InlineData("-123")]
        [InlineData((long)123)]
        [InlineData((float)123)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public void Converts_values_to_short(object value)
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Short").Returns(value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            short shortValue = Convert.ToInt16(value);
            result.Short.ShouldBe(shortValue);
        }

        [Theory]
        [InlineData(12345)]
        [InlineData("12345")]
        [InlineData("-12345")]
        [InlineData((long)123)]
        [InlineData((float)123)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Converts_values_to_int(object value)
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Int").Returns(value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            int intValue = Convert.ToInt32(value);
            result.Int.ShouldBe(intValue);
        }

        [Theory]
        [InlineData(1234567890)]
        [InlineData("1234567890")]
        [InlineData("-1234567890")]
        [InlineData((float)1234567890)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void Converts_values_to_long(object value)
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Long").Returns(value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            long longValue = Convert.ToInt64(value);
            result.Long.ShouldBe(longValue);
        }

        [Theory]
        [InlineData(12345.678)]
        [InlineData("12345.678")]
        [InlineData("-12345.678")]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        public void Converts_values_to_float(object value)
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Float").Returns(value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            float floatValue = Convert.ToSingle(value);
            result.Float.ShouldBe(floatValue);
        }

        [Theory]
        [InlineData(123456789.12345)]
        [InlineData("123456789.12345")]
        [InlineData("-123456789.12345")]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void Converts_values_to_double(object value)
        {
            var fakeContext = Substitute.For<IConfigurationContext>();
            fakeContext.Get("Double").Returns(value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            double doubleValue = Convert.ToDouble(value);
            result.Double.ShouldBe(doubleValue);
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
