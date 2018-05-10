using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Vivelin.AspNetCore.Headers
{
    public class HeaderDictionary : Dictionary<string, StringValues>, IHeaderDictionary
    {
        /// <summary>
        /// Strongly typed access to the Content-Length header.
        /// </summary>
        public long? ContentLength
        {
            get
            {
                if (long.TryParse(this["Content-Length"].FirstOrDefault(), out var result))
                    return result;
                return null;
            }
            set
            {
                this["Content-Length"] = value.ToString();
            }
        }
    }
}
