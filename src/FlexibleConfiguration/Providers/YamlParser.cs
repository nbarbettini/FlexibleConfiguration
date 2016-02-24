using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlexibleConfiguration.Abstractions;
using FlexibleConfiguration.Providers.YamlVisitors;
using YamlDotNet.RepresentationModel;

namespace FlexibleConfiguration.Providers
{
    public class YamlParser
    {
        private readonly string root;

        public YamlParser(string root = null)
        {
            this.root = root;
        }

        public IDictionary<string, string> Parse(Stream input)
        {
            IDictionary<string, string> data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            data.Clear();
            var visitor = new ContextAwareVisitor();

            try
            {
                var yamlStream = new YamlStream();

                using (var reader = new StreamReader(input))
                {
                    yamlStream.Load(reader);

                }

                if (!yamlStream.Documents.Any())
                {
                    return data;
                }

                yamlStream.Accept(visitor);
            }
            catch (YamlDotNet.Core.YamlException e)
            {
                throw new FormatException("Error parsing YAML.", e);
            }

            foreach (var item in visitor.Items)
            {
                var key = item.Key;

                if (!string.IsNullOrEmpty(this.root))
                {
                    key = ConfigurationPath.Combine(this.root, key);
                }

                if (data.ContainsKey(key))
                {
                    throw new FormatException(string.Format("The key '{0}' is duplicated.", key));
                }
                data[key] = item.Value;
            }

            return data;
        }
    }
}
