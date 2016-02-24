// Copyright (c) Nate Barbettini.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// Contains code modified from the ASP.NET Configuration project. Licensed under the Apache License, Version 2.0 from the .NET Foundation.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using FlexibleConfiguration.Abstractions;

namespace FlexibleConfiguration
{
    public static class ConfigurationBinder
    {
        public static void Bind<T>(this IConfiguration configuration, T instance, T defaultValue = null, BindingOptions bindingOptions = null)
            where T : class
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (bindingOptions == null)
            {
                bindingOptions = new BindingOptions();
            }

            IConfiguration targetConfigurationSection = configuration;

            if (!string.IsNullOrEmpty(bindingOptions.RootNode))
            {
                targetConfigurationSection = configuration.GetSection(bindingOptions.RootNode);
            }

            if (instance != null)
            {
                BindInstance(typeof(T), instance, defaultValue, targetConfigurationSection);
            }
        }

        public static T GetValue<T>(this IConfiguration configuration, string key)
        {
            return GetValue(configuration, key, default(T));
        }

        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            return (T)GetValue(configuration, typeof(T), key, defaultValue);
        }

        public static object GetValue(this IConfiguration configuration, Type type, string key)
        {
            return GetValue(configuration, type, key, defaultValue: null);
        }

        public static object GetValue(this IConfiguration configuration, Type type, string key, object defaultValue)
        {
            var value = configuration.GetSection(key).Value;
            if (value != null)
            {
                return ConvertValue(type, value);
            }
            return defaultValue;
        }

        private static void BindNonScalar(this IConfiguration configuration, object instance, object defaultValue)
        {
            if (instance != null)
            {
                foreach (var property in GetAllProperties(instance.GetType().GetTypeInfo()))
                {
                    BindProperty(property, instance, defaultValue, configuration);
                }
            }
        }

        private static void BindProperty(PropertyInfo property, object instance, object defaultValue, IConfiguration config)
        {
            // We don't support set only, non public, or indexer properties
            if (property.GetMethod == null ||
                !property.GetMethod.IsPublic ||
                property.GetMethod.GetParameters().Length > 0)
            {
                return;
            }

            var propertyValue = property.GetValue(instance);
            var hasAvailableSetter = property.SetMethod != null && (property.SetMethod.IsPublic || property.SetMethod.IsAssembly);

            if (propertyValue == null && !hasAvailableSetter)
            {
                // Property doesn't have a value and we cannot set it so there is no
                // point in going further down the graph
                return;
            }

            object defaultPropertyValue = null;
            if (defaultValue != null)
            {
                defaultPropertyValue = property.GetValue(defaultValue);
            }

            propertyValue = BindInstance(property.PropertyType, propertyValue, defaultPropertyValue, config.GetSection(property.Name));
            if (propertyValue != null && hasAvailableSetter)
            {
                property.SetValue(instance, propertyValue);
            }
        }

        private static object BindInstance(Type type, object instance, object defaultValue, IConfiguration config)
        {
            var section = config as IConfigurationSection;
            var configValue = section?.Value;
            if (configValue != null)
            {
                // Leaf nodes are always reinitialized
                return ConvertValue(type, configValue);
            }

            if (config != null && config.GetChildren().Any())
            {
                if (instance == null)
                {
                    instance = CreateInstance(type);
                }

                // See if it's a ReadOnlyDictionary
                var collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyDictionary<,>), type);
                if (collectionInterface != null)
                {
                    var genericDictionaryType = typeof(Dictionary<,>);
                    var actualDictionaryType = genericDictionaryType.MakeGenericType(type.GenericTypeArguments);
                    instance = Activator.CreateInstance(actualDictionaryType);
                    type = actualDictionaryType;
                }

                // See if its a Dictionary
                collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
                if (collectionInterface != null)
                {
                    BindDictionary(instance, collectionInterface, config);
                }
                else if (type.IsArray)
                {
                    instance = BindArray((Array)instance, config);
                }
                else
                {
                    // See if it's an IReadOnlyCollection
                    collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyCollection<>), type);
                    if (collectionInterface != null)
                    {
                        var genericListType = typeof(List<>);
                        var actualListType = genericListType.MakeGenericType(type.GenericTypeArguments);
                        instance = Activator.CreateInstance(actualListType);
                        type = actualListType;
                    }

                    // See if its an ICollection
                    collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
                    if (collectionInterface != null)
                    {
                        BindCollection(instance, collectionInterface, config);
                    }
                    // Something else
                    else
                    {
                        BindNonScalar(config, instance, defaultValue);
                    }
                }
            }
            else if (defaultValue != null)
            {
                return defaultValue;
            }

            return instance;
        }

        private static object CreateInstance(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsInterface || typeInfo.IsAbstract)
            {
                throw new InvalidOperationException("Cannot activate abstract or interface types.");
            }

            if (typeInfo.IsArray)
            {
                if (typeInfo.GetArrayRank() > 1)
                {
                    throw new InvalidOperationException("Unsupported multidimensional array.");
                }

                return Array.CreateInstance(typeInfo.GetElementType(), 0);
            }

            var defaultConstructor = typeInfo.DeclaredConstructors.FirstOrDefault(ctor => (ctor.IsPublic || ctor.IsAssembly) && ctor.GetParameters().Length == 0);
            if (defaultConstructor == null)
            {
                throw new InvalidOperationException($"Missing parameterless public constructor on type '{type.Name}'");
            }

            try
            {
                return defaultConstructor.Invoke(null);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create an instance of type '{type.Name}'", ex);
            }
        }

        private static void BindDictionary(object dictionary, Type dictionaryType, IConfiguration config)
        {
            var typeInfo = dictionaryType.GetTypeInfo();

            // IDictionary<K,V> is guaranteed to have exactly two parameters
            var keyType = typeInfo.GenericTypeArguments[0];
            var valueType = typeInfo.GenericTypeArguments[1];

            if (keyType != typeof(string))
            {
                // We only support string keys
                return;
            }

            var addMethod = typeInfo.GetDeclaredMethod("Add");
            foreach (var child in config.GetChildren())
            {
                var item = BindInstance(
                    type: valueType,
                    instance: null,
                    defaultValue: null, // todo
                    config: child);
                if (item != null)
                {
                    var key = child.Key;
                    addMethod.Invoke(dictionary, new[] { key, item });
                }
            }
        }

        private static void BindCollection(object collection, Type collectionType, IConfiguration config)
        {
            var typeInfo = collectionType.GetTypeInfo();

            // ICollection<T> is guaranteed to have exacly one parameter
            var itemType = typeInfo.GenericTypeArguments[0];
            var addMethod = typeInfo.GetDeclaredMethod("Add");

            foreach (var section in config.GetChildren())
            {
                try
                {
                    var item = BindInstance(
                        type: itemType,
                        instance: null,
                        defaultValue: null, // todo
                        config: section);
                    if (item != null)
                    {
                        addMethod.Invoke(collection, new[] { item });
                    }
                }
                catch
                {
                }
            }
        }

        private static Array BindArray(Array source, IConfiguration config)
        {
            var children = config.GetChildren().ToArray();
            var arrayLength = source.Length;
            var elementType = source.GetType().GetElementType();
            var newArray = Array.CreateInstance(elementType, arrayLength + children.Length);

            // binding to array has to preserve already initialized arrays with values
            if (arrayLength > 0)
            {
                Array.Copy(source, newArray, arrayLength);
            }

            for (int i = 0; i < children.Length; i++)
            {
                try
                {
                    var item = BindInstance(
                        type: elementType,
                        instance: null,
                        defaultValue: null, // todo
                        config: children[i]);
                    if (item != null)
                    {
                        newArray.SetValue(item, arrayLength + i);
                    }
                }
                catch
                {
                }
            }

            return newArray;
        }

        private static object ConvertValue(Type type, string value)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return string.IsNullOrEmpty(value)
                    ? null
                    : ConvertValue(Nullable.GetUnderlyingType(type), value);
            }

            try
            {
                return TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to bind value '{value}' on type '{type.Name}'", ex);
            }
        }

        private static Type FindOpenGenericInterface(Type expected, Type actual)
        {
            var actualTypeInfo = actual.GetTypeInfo();
            var interfaces = actualTypeInfo.ImplementedInterfaces.ToList();

            if (actualTypeInfo.IsInterface)
            {
                interfaces.Add(actual);
            }

            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.GetTypeInfo().IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == expected)
                {
                    return interfaceType;
                }
            }
            return null;
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type)
        {
            var allProperties = new List<PropertyInfo>();

            do
            {
                allProperties.AddRange(type.DeclaredProperties);
                type = type.BaseType.GetTypeInfo();
            }
            while (type != typeof(object).GetTypeInfo());

            return allProperties;
        }
    }
}