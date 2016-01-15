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
    }
}
