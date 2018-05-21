using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Represents an ASP.NET Core middleware that adds headers to an HTTP response.
    /// </summary>
    public class ResponseHeadersMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ResponseHeadersOptions options;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ResponseHeadersMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// A function used to continue processing of an HTTP request.
        /// </param>
        /// <param name="loggerFactory">
        /// A type used to provide <see cref="ILogger"/> instances.
        /// </param>
        /// <param name="options">
        /// A container used to specify the options for the middleware.
        /// </param>
        public ResponseHeadersMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<ResponseHeadersOptions> options)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            //if (loggerFactory == null)
            //    throw new ArgumentNullException(nameof(loggerFactory));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.next = next;
            this.options = options.Value;
        }

        /// <summary>
        /// Executes the middleware on the specified request.
        /// </summary>
        /// <param name="context">
        /// A <see cref="HttpContext"/> object that represents the current request.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task InvokeAsync(HttpContext context)
        {
            AddResponseHeaders(context);
            return next(context);
        }

        /// <summary>
        /// Adds the configured HTTP headers to the response.
        /// </summary>
        /// <param name="context">
        /// A <see cref="HttpContext"/> object that represents the current request.
        /// </param>
        public void AddResponseHeaders(HttpContext context)
        {
            foreach (var item in options.Headers)
                context.Response.Headers.Add(item);
        }
    }
}