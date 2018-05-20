using System;
using System.Linq;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides a set of static methods for configuring a Content Security Policy.
    /// </summary>
    public static class ContentSecurityPolicyExtensions
    {
        /// <summary>
        /// Specifies a Content Security Policy.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <param name="value">A string that contains the serialized policy.</param>
        /// <returns>
        /// A reference to <paramref name="builder"/> with the specified Content
        /// Security Policy.
        /// </returns>
        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, string value)
        {
            return builder.Add("Content-Security-Policy", value);
        }

        /// <summary>
        /// Specifies a Content Security Policy using the specified configuration action.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <param name="optionsAction">
        /// An action used to configure the Content Security Policy.
        /// </param>
        /// <returns>
        /// A reference to <paramref name="builder"/> with the specified Content
        /// Security Policy.
        /// </returns>
        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, Action<ContentSecurityPolicyBuilder> optionsAction)
        {
            var cspBuilder = new ContentSecurityPolicyBuilder();
            optionsAction(cspBuilder);

            var directives = cspBuilder.Build()
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrEmpty(x));
            return builder.AddContentSecurityPolicy(string.Join(" ; ", directives));
        }
    }
}