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
        private readonly string breadcrumb;

        public TargetBuilder(Type type, IConfigurationContext config)
            : this(type, config, null)
        {
        }

        private TargetBuilder(Type type, IConfigurationContext context, string breadcrumb)
        {
            ThrowIfInvalidTarget(type);

            this.type = type;
            this.context = context;
            this.breadcrumb = breadcrumb;
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
                        prop.SetValue(obj, valueFromContext);
                    }
                }
                else
                {
                    var childBuilder = new TargetBuilder(prop.PropertyType, this.context, this.GetFullyQualifiedName(prop.Name));
                    prop.SetValue(obj, childBuilder.Build());
                }
            }
        }

        private string GetFullyQualifiedName(string name)
        {
            if (string.IsNullOrEmpty(this.breadcrumb))
            {
                return name;
            }

            return $"{this.breadcrumb}.{name}";
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
