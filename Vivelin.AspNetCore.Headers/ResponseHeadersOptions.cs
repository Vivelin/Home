using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Vivelin.AspNetCore.Headers
{
    public class ResponseHeadersOptions
    {
        public ResponseHeadersOptions()
        {
            Headers = new HeaderDictionary();
        }

        public IHeaderDictionary Headers { get; }
    }
}
