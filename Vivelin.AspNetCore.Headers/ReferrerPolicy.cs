namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Specifies the algorithm used to populate the <c>Referrer</c> header when
    /// fetching subresources, prefeteching, or performing navigations.
    /// </summary>
    public sealed class ReferrerPolicy
    {
        private readonly string value;

        /// <summary>
        /// Specifies that the referrer policy is defined elsewhere.
        /// </summary>
        public static readonly ReferrerPolicy None = new ReferrerPolicy("");

        /// <summary>
        /// Specifies that no referrer information is to be sent along with
        /// requests to any origin.
        /// </summary>
        public static readonly ReferrerPolicy NoReferrer = new ReferrerPolicy("no-referrer");

        /// <summary>
        /// Specifies that a full URL is sent as referrer information. However,
        /// requests from HTTPS to a non-potentially trustworthy URL will contain
        /// no referrer information.
        /// </summary>
        public static readonly ReferrerPolicy NoReferrerWhenDowngrade = new ReferrerPolicy("no-referrer-when-downgrade");

        /// <summary>
        /// Specifies that a full URL is sent as referrer information when making
        /// same-origin requests. Cross-origin requests will contain no referrer information.
        /// </summary>
        public static readonly ReferrerPolicy SameOrigin = new ReferrerPolicy("same-origin");

        /// <summary>
        /// Specifies that only the origin of the request is sent as referrer
        /// information when making both same-origin requests and cross-origin requests.
        /// </summary>
        public static readonly ReferrerPolicy Origin = new ReferrerPolicy("origin");

        /// <summary>
        /// Specifies that only the origin of the request is sent as referrer
        /// information when making both same-origin requests and cross-origin
        /// requests. However, requests from HTTPS to a non-potentially
        /// trustworthy URL will contain no referrer information.
        /// </summary>
        public static readonly ReferrerPolicy StrictOrigin = new ReferrerPolicy("strict-origin");

        /// <summary>
        /// Specifies that a full URL is sent as referrer information when making
        /// same-origin requests, and that only the origin of the request is sent
        /// as referrer information when making cross-origin requests.
        /// </summary>
        public static readonly ReferrerPolicy OriginWhenCrossOrigin = new ReferrerPolicy("origin-when-cross-origin");

        /// <summary>
        /// Specifies that a full URL is sent as referrer information when making
        /// same-origin requests, and that only the origin of the request is sent
        /// as referrer information when making cross-origin requests. However,
        /// requests from HTTPS to a non-potentially trustworthy URL will contain
        /// no referrer information.
        /// </summary>
        public static readonly ReferrerPolicy StrictOriginWhenCrossOrigin = new ReferrerPolicy("strict-origin-when-cross-origin");

        /// <summary>
        /// Specifies that a full URL is sent as referrer information with both
        /// cross-origin and same-origin requests.
        /// </summary>
        public static readonly ReferrerPolicy UnsafeUrl = new ReferrerPolicy("unsafe-url");

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferrerPolicy"/> class
        /// with the specified string value.
        /// </summary>
        /// <param name="value">A string that identifies the referrer policy.</param>
        public ReferrerPolicy(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Returns the string value as a <see cref="ReferrerPolicy"/> object.
        /// </summary>
        /// <param name="value">A string value to convert.</param>
        public static implicit operator ReferrerPolicy(string value)
        {
            return new ReferrerPolicy(value);
        }

        /// <summary>
        /// Returns a string that represents the referrer policy.
        /// </summary>
        /// <param name="value">The referrer policy whose string value to return.</param>
        public static implicit operator string(ReferrerPolicy value)
        {
            return value?.value;
        }

        /// <summary>
        /// Returns a string that represents the referrer policy.
        /// </summary>
        /// <returns>A string that represents the referrer policy.</returns>
        public override string ToString()
        {
            return value;
        }
    }
}