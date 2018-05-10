namespace Vivelin.AspNetCore.Headers
{
    public static class ContentSecurityPolicyExtensions
    {
        public static ResponseHeadersOptionsBuilder AddContentSecurityPolicy(this ResponseHeadersOptionsBuilder builder, params string[] values)
        {
            return builder.Add("Content-Security-Policy", values);
        }
    }
}
