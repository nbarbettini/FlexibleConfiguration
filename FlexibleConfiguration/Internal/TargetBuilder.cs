// <copyright file="TargetBuilder.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace FlexibleConfiguration.Internal
{
    internal sealed class TargetBuilder
    {
        private readonly Type type;
        private readonly IConfigurationContext context;
        private readonly string root;

        public TargetBuilder(Type type, IConfigurationContext config)
            : this(type, config, null)
        {
        }

        private TargetBuilder(Type type, IConfigurationContext context, string root)
        {
            ThrowIfInvalidTarget(type);

            this.type = type;
            this.context = context;
            this.root = root;
        }

        public object Build()
        {
            var instance = Activator.CreateInstance(this.type);

            this.SetProperties(instance);

            return instance;
        }

        private void SetProperties(object obj)
        {
            var properties = this.type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (IsSupportedPrimitive(prop.PropertyType))
                {
                    var valueFromContext = this.context.Get(this.GetFullyQualifiedName(prop.Name));
                    if (valueFromContext != null)
                    {
                        try
                        {
                            prop.SetValue(obj, CoerceTo(prop.PropertyType, valueFromContext), null);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"Could not set value '{valueFromContext}' for configuration property '{prop.Name}'. See the inner exception for details.", ex);
                        }
                    }
                }
                else
                {
                    var childBuilder = new TargetBuilder(prop.PropertyType, this.context, this.GetFullyQualifiedName(prop.Name));
                    prop.SetValue(obj, childBuilder.Build(), null);
                }
            }
        }

        private string GetFullyQualifiedName(string name)
        {
            if (string.IsNullOrEmpty(this.root))
            {
                return name;
            }

            return $"{this.root}.{name}";
        }

        private static object CoerceTo(Type targetType, object value)
        {
            if (targetType == typeof(short))
            {
                return Convert.ToInt16(value);
            }

            if (targetType == typeof(int))
            {
                return Convert.ToInt32(value);
            }

            if (targetType == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (targetType == typeof(float))
            {
                return Convert.ToSingle(value);
            }

            if (targetType == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            if (targetType == typeof(string))
            {
                return value.ToString();
            }

            throw new ValidationException($"The type '{targetType.Name}' is not a supported configuration target type.");
        }

        private static void ThrowIfInvalidTarget(Type targetType)
        {
            if (targetType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new ArgumentException("The type must contain a constructor with no arguments.");
            }
        }

        private static bool IsSupportedPrimitive(Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                type == typeof(string);
        }
    }
}
