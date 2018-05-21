using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides a set of static methods for configuring the response header middleware.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Specifies additional HTTP headers to add to responses.
        /// </summary>
        /// <param name="app">The builder to configure.</param>
        /// <param name="optionsAction">
        /// An action used to specify the headers to add.
        /// </param>
        /// <returns>
        /// A reference to <paramref name="app"/> with the configured response headers.
        /// </returns>
        public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app, Action<ResponseHeadersOptionsBuilder> optionsAction)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (optionsAction == null)
                throw new ArgumentNullException(nameof(optionsAction));

            var builder = new ResponseHeadersOptionsBuilder();
            optionsAction(builder);
            return app.UseResponseHeaders(builder.Build());
        }

        /// <summary>
        /// Specifies additional HTTP headers to add to responses.
        /// </summary>
        /// <param name="app">The builder to configure.</param>
        /// <param name="options">Specifies the response headers to add.</param>
        /// <returns>
        /// A reference to <paramref name="app"/> with the configured response headers.
        /// </returns>
        public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app, ResponseHeadersOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return app.UseMiddleware<ResponseHeadersMiddleware>(Options.Create(options));
        }
    }
}