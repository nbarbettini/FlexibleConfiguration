// <copyright file="PropertyEnumerator.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleConfiguration
{
    internal sealed class PropertyEnumerator
    {
        private readonly Type type;
        private readonly string breadcrumb;

        public PropertyEnumerator(Type type)
            : this(type, null)
        {
        }

        public PropertyEnumerator(Type type, params string[] breadcrumb)
        {
            this.type = type;

            var removedEmptyEntries = breadcrumb?.Where(x => !string.IsNullOrEmpty(x));
            if (removedEmptyEntries == null || !removedEmptyEntries.Any())
            {
                this.breadcrumb = string.Empty;
            }
            else
            {
                this.breadcrumb = string.Join(".", removedEmptyEntries);
            }
        }

        public IEnumerable<string> GetProperties()
        {
            var result = new List<string>();

            var properties = this.type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (IsSupportedPrimitive(prop.PropertyType))
                {
                    result.Add(this.GetFullyQualifiedName(prop.Name));
                }
                else
                {
                    var child = new PropertyEnumerator(prop.PropertyType, this.breadcrumb, prop.Name);
                    result.AddRange(child.GetProperties());
                }
            }

            return result;
        }

        private string GetFullyQualifiedName(string name)
        {
            if (string.IsNullOrEmpty(this.breadcrumb))
            {
                return name;
            }

            return $"{this.breadcrumb}.{name}";
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
