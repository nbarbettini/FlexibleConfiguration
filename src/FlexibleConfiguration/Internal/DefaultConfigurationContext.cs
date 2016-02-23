// <copyright file="DefaultConfigurationContext.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Map = System.Collections.Generic.IDictionary<string, object>;

namespace FlexibleConfiguration.Internal
{
    public sealed class DefaultConfigurationContext : IConfigurationContext
    {
        private readonly Map configMap;

        public DefaultConfigurationContext()
        {
            this.configMap = new Dictionary<string, object>();
        }

        public void Put(string fullyQualifiedPath, object value)
        {
            if (string.IsNullOrEmpty(fullyQualifiedPath))
            {
                throw new ArgumentNullException(nameof(fullyQualifiedPath));
            }

            var path = fullyQualifiedPath
                .ToLower()
                .Split('.');
            Map currentLevel = this.configMap;

            if (path.Length > 1)
            {
                foreach (var token in path.TakeWhile((_, index) => index < path.Length))
                {
                    object valueAtLevel;
                    if (currentLevel.TryGetValue(token, out valueAtLevel))
                    {
                        if (valueAtLevel != null)
                        {
                            if (valueAtLevel is Map)
                            {
                                currentLevel = (Map)valueAtLevel;
                                continue;
                            }
                            else
                            {
                                throw new ParseException($"Element '{token}' is a value and cannot contain children.");
                            }
                        }
                    }

                    var newMap = new Dictionary<string, object>();
                    currentLevel[token] = newMap;
                    currentLevel = newMap;
                }
            }

            currentLevel[path.Last()] = value;
        }

        public string Get(string fullyQualifiedPath)
        {
            if (string.IsNullOrEmpty(fullyQualifiedPath))
            {
                return null;
            }

            var path = fullyQualifiedPath
                .ToLower()
                .Split('.');
            Map currentLevel = this.configMap;

            if (path.Length > 1)
            {
                foreach (var token in path.TakeWhile((_, index) => index < path.Length))
                {
                    object valueAtLevel = null;
                    if (currentLevel.TryGetValue(token, out valueAtLevel))
                    {
                        if (valueAtLevel != null)
                        {
                            if (valueAtLevel is Map)
                            {
                                currentLevel = (Map)valueAtLevel;
                                continue;
                            }
                            else
                            {
                                throw new ParseException($"Expecting to find a node named {token}; found a value instead.");
                            }
                        }
                    }

                    if (valueAtLevel == null)
                    {
                        return null;
                    }
                }
            }

            object value = null;
            currentLevel.TryGetValue(path.Last(), out value);
            return value?.ToString();
        }

        public void Remove(string fullyQualifiedPath)
        {
            this.Put(fullyQualifiedPath, null);
        }

        public string GetString(string fullyQualifiedPath)
        {
            var value = this.Get(fullyQualifiedPath);

            return value?.ToString();
        }

        public bool Exists(string fullyQualifiedPath)
        {
            var value = this.Get(fullyQualifiedPath);

            return value != null;
        }
    }
}
