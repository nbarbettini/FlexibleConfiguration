// <copyright file="TargetBuilder_types_tests.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using FlexibleConfiguration.Internal;
using Shouldly;
using Xunit;

namespace FlexibleConfiguration.Tests
{
    public class TargetBuilder_types_tests
    {
        public static IEnumerable<object[]> GetShortCases()
        {
            yield return new object[] { 123 };
            yield return new object[] { "123" };
            yield return new object[] { "-123" };
            yield return new object[] { short.MaxValue };
            yield return new object[] { short.MinValue };
        }

        public static IEnumerable<object[]> GetIntCases()
        {
            yield return new object[] { 12345 };
            yield return new object[] { "12345" };
            yield return new object[] { "-12345" };
            yield return new object[] { int.MaxValue };
            yield return new object[] { int.MinValue };
        }

        public static IEnumerable<object[]> GetLongCases()
        {
            yield return new object[] { 1234567890 };
            yield return new object[] { "1234567890" };
            yield return new object[] { "-1234567890" };
            yield return new object[] { long.MaxValue };
            yield return new object[] { long.MinValue };
        }

        public static IEnumerable<object[]> GetFloatCases()
        {
            yield return new object[] { 12345.678 };
            yield return new object[] { "12345.678" };
            yield return new object[] { "-12345.678" };
        }

        public static IEnumerable<object[]> GetDoubleCases()
        {
            yield return new object[] { 123456789.12345 };
            yield return new object[] { "123456789.12345" };
            yield return new object[] { "-123456789.12345" };
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        public void Converts_values_to_short(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Short", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            short shortValue = Convert.ToInt16(value);
            result.Short.ShouldBe(shortValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        public void Converts_values_to_int(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Int", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            int intValue = Convert.ToInt32(value);
            result.Int.ShouldBe(intValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        public void Converts_values_to_long(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Long", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            long longValue = Convert.ToInt64(value);
            result.Long.ShouldBe(longValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        [MemberData(nameof(GetFloatCases))]
        public void Converts_values_to_float(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Float", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            float floatValue = Convert.ToSingle(value);
            result.Float.ShouldBe(floatValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        [MemberData(nameof(GetFloatCases))]
        [MemberData(nameof(GetDoubleCases))]
        public void Converts_values_to_double(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("Double", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            double doubleValue = Convert.ToDouble(value);
            result.Double.ShouldBe(doubleValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [InlineData(null)]
        public void Converts_values_to_nullable_short(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("NullableShort", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            short? shortValue = value == null
                ? (short?)null
                : Convert.ToInt16(value);
            result.NullableShort.ShouldBe(shortValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [InlineData(null)]
        public void Converts_values_to_nullable_int(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("NullableInt", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            int? intValue = value == null
                ? (int?)null
                : Convert.ToInt32(value);
            result.NullableInt.ShouldBe(intValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        [InlineData(null)]
        public void Converts_values_to_nullable_long(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("NullableLong", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            long? longValue = value == null
                ? (long?)null
                : Convert.ToInt64(value);
            result.NullableLong.ShouldBe(longValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        [MemberData(nameof(GetFloatCases))]
        [InlineData(null)]
        public void Converts_values_to_nullable_float(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("NullableFloat", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            float? floatValue = value == null
                ? (float?)null
                : Convert.ToSingle(value);
            result.NullableFloat.ShouldBe(floatValue);
        }

        [Theory]
        [MemberData(nameof(GetShortCases))]
        [MemberData(nameof(GetIntCases))]
        [MemberData(nameof(GetLongCases))]
        [MemberData(nameof(GetFloatCases))]
        [MemberData(nameof(GetDoubleCases))]
        [InlineData(null)]
        public void Converts_values_to_nullable_double(object value)
        {
            var fakeContext = new DefaultConfigurationContext();
            fakeContext.Put("NullableDouble", value);

            var builder = new TargetBuilder(typeof(TypesConfig), fakeContext);
            var result = (TypesConfig)builder.Build();

            double? doubleValue = value == null
                ? (double?)null
                : Convert.ToDouble(value);
            result.NullableDouble.ShouldBe(doubleValue);
        }
    }
}
