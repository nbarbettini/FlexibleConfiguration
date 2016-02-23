// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Reflection;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers.ObjectVisitors
{
    public abstract class ObjectVisitor
    {
        protected virtual void VisitObject(object obj)
        {
            if (obj != null)
            {
                VisitProperties(obj);
            }
        }

        protected virtual void VisitProperties(object obj)
        {
            foreach (var property in obj.GetType().GetTypeInfo().GetAllProperties())
            {
                VisitProperty(property.Name, property.PropertyType.GetTypeInfo(), property.GetValue(obj));
            }
        }

        protected virtual void VisitProperty(string name, TypeInfo propertyTypeInfo, object actualValue)
        {
            if (IsSupportedPrimitive(propertyTypeInfo))
            {
                VisitPrimitive(actualValue);
            }
            else if (typeof(IDictionary).GetTypeInfo().IsAssignableFrom(propertyTypeInfo))
            {
                VisitDictionary(actualValue as IDictionary);
            }
            else if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(propertyTypeInfo))
            {
                VisitEnumerable(actualValue as IEnumerable);
            }
            else if (propertyTypeInfo.IsClass)
            {
                VisitObject(actualValue);
            }
            else
            {
                throw new NotSupportedException($"The type '{propertyTypeInfo.Name}' is not supported at this position.");
            }
        }

        protected virtual void VisitPrimitive(object primitiveValue)
        {
            // Do nothing.
        }

        protected virtual void VisitEnumerable(IEnumerable enumerable)
        {
            // Do nothing.
        }

        protected virtual void VisitDictionary(IDictionary dictionary)
        {
            // Do nothing.
        }

        protected static bool IsSupportedPrimitive(TypeInfo typeInfo)
        {
            return typeInfo.IsPrimitive
                || typeInfo.IsEnum
                || typeInfo == typeof(string).GetTypeInfo()
                || IsNullable(typeInfo.AsType());
        }

        private static bool IsNullable(Type possiblyNullable)
            => Nullable.GetUnderlyingType(possiblyNullable) != null;
    }
}
