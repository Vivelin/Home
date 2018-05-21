using System;
using Microsoft.AspNetCore.Http;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides configuration for the headers to add to a response.
    /// </summary>
    public class ResponseHeadersOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeadersOptions"/> class.
        /// </summary>
        public ResponseHeadersOptions()
        {
            Headers = new HeaderDictionary();
        }

        /// <summary>
        /// Gets a dictionary that contain the headers to add to a response.
        /// </summary>
        public IHeaderDictionary Headers { get; }
    }
}