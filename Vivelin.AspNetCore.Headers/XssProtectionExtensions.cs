namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides a set of static methods for configuring a user agent's cross-site
    /// scripting protection.
    /// </summary>
    public static class XssProtectionExtensions
    {
        private const string Disable = "0";
        private const string Enable = "1";
        private const string EnableAndBlock = "1; mode=block";

        /// <summary>
        /// Specifies the value for the <c>X-XSS-Protection</c> header.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <param name="value">The <c>X-XSS-Protection</c> header value.</param>
        /// <returns>
        /// A reference to <paramref name="builder"/> with the specified XSS
        /// protection value.
        /// </returns>
        public static ResponseHeadersOptionsBuilder AddXssProtection(this ResponseHeadersOptionsBuilder builder, string value)
        {
            return builder.Add("X-XSS-Protection", value);
        }

        /// <summary>
        /// Specifies that user agents should disable XSS filtering,
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <returns>
        /// A reference to <paramref name="builder"/> without XSS protection.
        /// </returns>
        public static ResponseHeadersOptionsBuilder DisableXssProtection(this ResponseHeadersOptionsBuilder builder)
        {
            return builder.AddXssProtection(Disable);
        }

        /// <summary>
        /// Specifies that user agents should enable XSS filtering.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <param name="block">
        /// Indicates whether pages should be blocked rather than sanitized when
        /// an XSS attack is detected.
        /// </param>
        /// <returns>
        /// A reference to <paramref name="builder"/> with the specified XSS
        /// protection mode.
        /// </returns>
        public static ResponseHeadersOptionsBuilder AddXssProtection(this ResponseHeadersOptionsBuilder builder, bool block = false)
        {
            return builder.AddXssProtection(block ? EnableAndBlock : Enable);
        }
    }
}