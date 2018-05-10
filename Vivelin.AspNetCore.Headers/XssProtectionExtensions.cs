namespace Vivelin.AspNetCore.Headers
{
    public static class XssProtectionExtensions
    {
        private const string Disable = "0";
        private const string Enable = "1";
        private const string EnableAndBlock = "1; mode=block";

        public static ResponseHeadersOptionsBuilder AddXssProtection(this ResponseHeadersOptionsBuilder builder, string value)
        {
            return builder.Add("X-XSS-Protection", value);
        }

        public static ResponseHeadersOptionsBuilder DisableXssProtection(this ResponseHeadersOptionsBuilder builder)
        {
            return builder.AddXssProtection(Disable);
        }

        public static ResponseHeadersOptionsBuilder AddXssProtection(this ResponseHeadersOptionsBuilder builder, bool block = false)
        {
            return builder.AddXssProtection(block ? EnableAndBlock : Enable);
        }
    }
}
