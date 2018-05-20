using System;
using System.Linq;

namespace Vivelin.AspNetCore.Headers
{
    public static class ContentSecurityPolicyExtensions
    {
        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, string value)
        {
            return builder.Add("Content-Security-Policy", value);
        }

        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, params ContentSecurityPolicyDirective[] directives)
        {
            var value = string.Join(" ; ", directives.Select(x => x.ToString()).Where(x => !string.IsNullOrEmpty(x)));
            return builder.AddContentSecurityPolicy(value);
        }

        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, Action<ContentSecurityPolicyBuilder> optionsAction)
        {
            var cspBuilder = new ContentSecurityPolicyBuilder();
            optionsAction(cspBuilder);
            return builder.AddContentSecurityPolicy(cspBuilder);
        }
    }
}
