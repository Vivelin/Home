namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides a set of status methods for specifying a Referrer policy.
    /// </summary>
    public static class ReferrerPolicyExtensions
    {
        /// <summary>
        /// Specifies a policy that determines when the <c>Referer</c> header is sent.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to configure the response headers.
        /// </param>
        /// <param name="referrerPolicy">
        /// A value that specifies the algorithm used to populate the
        /// <c>Referer</c> header.
        /// </param>
        /// <returns>
        /// A reference to <paramref name="builder"/> with the specified Referrer Policy.
        /// </returns>
        public static ResponseHeadersOptionsBuilder AddReferrerPolicy(this ResponseHeadersOptionsBuilder builder, ReferrerPolicy referrerPolicy)
        {
            return builder.Add("Referrer-Policy", referrerPolicy.ToString());
        }
    }
}