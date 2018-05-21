using System;
using Microsoft.Extensions.Primitives;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides an API for configuring the headers to add to a response.
    /// </summary>
    public class ResponseHeadersOptionsBuilder
    {
        private readonly ResponseHeadersOptions options;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ResponseHeadersOptionsBuilder"/> class.
        /// </summary>
        public ResponseHeadersOptionsBuilder()
        {
            options = new ResponseHeadersOptions();
        }

        /// <summary>
        /// Adds a header with the specified value(s).
        /// </summary>
        /// <param name="name">The name of the HTTP header to add.</param>
        /// <param name="value">The value(s) of the HTTP header.</param>
        /// <returns>A reference to the builder.</returns>
        public ResponseHeadersOptionsBuilder Add(string name, StringValues value)
        {
            options.Headers.Add(name, value);
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="ResponseHeadersOptions"/> with the configured options.
        /// </summary>
        /// <returns>
        /// A new <see cref="ResponseHeadersOptions"/> with the configured options.
        /// </returns>
        public ResponseHeadersOptions Build()
        {
            return options;
        }
    }
}