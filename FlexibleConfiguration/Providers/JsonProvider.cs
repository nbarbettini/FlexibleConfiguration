// <copyright file="JsonProvider.cs" company="Nate Barbettini">
// Copyright (c) Nate Barbettini. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FlexibleConfiguration.Providers
{
    internal sealed class JsonProvider : AbstractConfigurationProvider
    {
        private readonly string json;
        private readonly string root;

        public JsonProvider(string json, string root)
        {
            this.json = json;
            this.root = root;
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            if (string.IsNullOrEmpty(this.json))
            {
                return Enumerable.Empty<KeyValuePair<string, object>>();
            }

            JObject deserialized = null;
            try
            {
                deserialized = JsonConvert.DeserializeObject(this.json) as JObject;

            }
            catch (Exception ex)
            {
                throw new ParseException("Error parsing JSON. See the InnerException for details.", ex);
            }

            if (deserialized == null)
            {
                throw new ParseException("Error parsing JSON. The result is null.");
            }

            var enumerator = new JsonEnumerator(deserialized, this.root);
            return enumerator.GetItems();
        }
    }
}
