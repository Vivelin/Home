﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Vivelin.AspNetCore.Headers
{
    public static class ApplicationBuilderExtensions
    {
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
