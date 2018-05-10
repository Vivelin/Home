namespace Vivelin.AspNetCore.Headers
{
    public static class XssProtectionExtensions
    {
        public static ResponseHeadersOptionsBuilder AddXssProtection(this ResponseHeadersOptionsBuilder builder, string value)
        {
            return builder.Add("X-XSS-Protection", value);
        }
    }
}
