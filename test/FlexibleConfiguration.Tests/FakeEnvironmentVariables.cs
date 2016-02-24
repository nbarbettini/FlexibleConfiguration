using System;
using System.Collections;
using FlexibleConfiguration.Internal;

namespace FlexibleConfiguration.Tests
{
    public class FakeEnvironmentVariables : IEnvironmentVariables
    {
        private readonly IDictionary variables;

        public FakeEnvironmentVariables(IDictionary variables = null)
        {
            this.variables = variables;
        }

        public string ExpandEnvironmentVariables(string name)
        {
            throw new NotImplementedException();
        }

        public string GetEnvironmentVariable(string variable)
        {
            throw new NotImplementedException();
        }

        public IDictionary GetEnvironmentVariables() => this.variables;
    }
}
