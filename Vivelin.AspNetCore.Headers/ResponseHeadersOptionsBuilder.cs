using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace Vivelin.AspNetCore.Headers
{
    public class ResponseHeadersOptionsBuilder
    {
        private readonly ResponseHeadersOptions options;

        public ResponseHeadersOptionsBuilder()
        {
            options = new ResponseHeadersOptions();
        }

        public ResponseHeadersOptionsBuilder Add(string name, StringValues value)
        {
            options.Headers.Add(name, value);
            return this;
        }

        public ResponseHeadersOptions Build()
        {
            return options;
        }
    }
}
