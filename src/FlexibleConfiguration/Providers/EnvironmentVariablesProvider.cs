// <copyright file="EnvironmentVariablesProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Providers
{
    internal sealed class EnvironmentVariablesProvider : AbstractConfigurationProvider
    {
        private readonly IEnvironmentVariables environment;
        private readonly EnvironmentVariableTarget target;

        private readonly IEnumerable<string> fullyQualifiedPathsToLookFor;
        private readonly string prefix;

        public EnvironmentVariablesProvider(IEnvironmentVariables environment, EnvironmentVariableTarget target, IEnumerable<string> fullyQualifiedPathsToLookFor, string prefix)
        {
            this.environment = environment;
            this.target = target;
            this.fullyQualifiedPathsToLookFor = fullyQualifiedPathsToLookFor;
            this.prefix = prefix;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetItems()
        {
            var keysToLookFor = this.fullyQualifiedPathsToLookFor
                .Select(x =>
                {
                    var key = x.Replace('.', '_');

                    if (!string.IsNullOrEmpty(this.prefix))
                    {
                        key = $"{this.prefix}_{key}";
                    }

                    return key;
                })
                .ToList();

            foreach (DictionaryEntry variable in this.environment.GetEnvironmentVariables(this.target))
            {
                var variableName = variable.Key.ToString();

                if (keysToLookFor.Contains(variableName, StringComparer.OrdinalIgnoreCase))
                {
                    yield return new KeyValuePair<string, string>(
                        this.ConvertToKey(variableName), variable.Value?.ToString());
                }
            }
        }

        private string ConvertToKey(string environmentVariableName)
        {
            if (!string.IsNullOrEmpty(this.prefix)
                && environmentVariableName.ToLower().IndexOf(this.prefix.ToLower()) == 0)
            {
                environmentVariableName = environmentVariableName.Substring(this.prefix.Length + 1);
            }

            return environmentVariableName
                .TrimStart('_')
                .Replace('_', '.');
        }
    }
}
