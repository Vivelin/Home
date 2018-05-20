using System;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides an API for configurating a Content Security Policy directive.
    /// </summary>
    public class ContentSecurityPolicyDirectiveBuilder
    {
        private const string None = "'none'";
        private const string Self = "'self'";
        private const string StrictDynamic = "'strict-dynamic'";
        private const string UnsafeEval = "'unsafe-eval'";
        private const string UnsafeInline = "'unsafe-inline'";
        private readonly ContentSecurityPolicyDirective directive;

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ContentSecurityPolicyDirectiveBuilder"/> class with the
        /// specified directive name.
        /// </summary>
        /// <param name="name">The name of a fetch directive.</param>
        public ContentSecurityPolicyDirectiveBuilder(string name)
        {
            directive = new ContentSecurityPolicyDirective(name);
        }

        /// <summary>
        /// Returns the <see cref="ContentSecurityPolicyDirective"/> constructed
        /// by the builder.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="ContentSecurityPolicyDirectiveBuilder"/> whose directive
        /// to return.
        /// </param>
        public static implicit operator ContentSecurityPolicyDirective(ContentSecurityPolicyDirectiveBuilder builder)
        {
            return builder.Build();
        }

        /// <summary>
        /// Adds a value to the fetch directive.
        /// </summary>
        /// <param name="value">The value to add to the directive.</param>
        /// <returns>A reference to the current instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Adding additional values to a directive after disallowing all requests
        /// will have no effect.
        /// </exception>
        public ContentSecurityPolicyDirectiveBuilder Add(string value)
        {
            if (directive.Values.Contains(None))
                throw new InvalidOperationException("Adding additional values to a directive after disallowing all requests will have no effect.");

            directive.Values.Add(value);
            return this;
        }

        /// <summary>
        /// Specifies that the unsafe evaluation of scripts or styles is allowed.
        /// </summary>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowEval() => Add(UnsafeEval);

        /// <summary>
        /// Specifies that everything on the specified origin is allowed.
        /// </summary>
        /// <param name="origin">The origin to allow, e.g. <c>https://example.com/</c>.</param>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowFromOrigin(string origin) => Add(origin);

        /// <summary>
        /// Specifies that any resource with the specified scheme is allowed.
        /// </summary>
        /// <param name="scheme">The scheme to allow, e.g. <c>data:</c>.</param>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowFromScheme(string scheme) => Add(scheme);

        /// <summary>
        /// Specifies that everything on the current origin is allowed.
        /// </summary>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowFromSelf() => Add(Self);

        /// <summary>
        /// Specifies that the specified URL is allowed.
        /// </summary>
        /// <param name="url">The full URL to allow, e.g. <c>https://example.com/path/to/file.js</c>.</param>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowFromUrl(string url) => Add(url);

        /// <summary>
        /// Specifies that requests from the specified host are allowed,
        /// regardless of scheme.
        /// </summary>
        /// <param name="host">
        /// The host to allow, e.g. <c>example.com</c> or <c>*.example.com</c>.
        /// </param>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowHost(string host) => Add(host);

        /// <summary>
        /// Specifies that unsafe execution of inline scripts or styles is allowed.
        /// </summary>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder AllowInline() => Add(UnsafeInline);

        /// <summary>
        /// Specifies that nothing is allowed.
        /// </summary>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder DisallowAll() => Add(None);

        /// <summary>
        /// Specifies the 'strict-dynamic' expression.
        /// </summary>
        /// <returns>A reference to the current instance.</returns>
        public ContentSecurityPolicyDirectiveBuilder Dynamic() => Add(StrictDynamic);

        /// <summary>
        /// Returns the <see cref="ContentSecurityPolicyDirective"/> constructed
        /// by the builder.
        /// </summary>
        /// <returns>
        /// A <see cref="ContentSecurityPolicyDirective"/> constructed by the builder.
        /// </returns>
        protected internal ContentSecurityPolicyDirective Build()
        {
            return directive;
        }
    }
}