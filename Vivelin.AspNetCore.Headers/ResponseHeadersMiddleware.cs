using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Vivelin.AspNetCore.Headers
{
    public class ResponseHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ResponseHeadersOptions options;

        public ResponseHeadersMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<ResponseHeadersOptions> options)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.next = next;
            this.options = options.Value;
        }

        public Task InvokeAsync(HttpContext context)
        {
            AddResponseHeaders(context);
            return next(context);
        }

        public void AddResponseHeaders(HttpContext context)
        {
            foreach (var item in options.Headers)
                context.Response.Headers.Add(item);
        }
    }
}
