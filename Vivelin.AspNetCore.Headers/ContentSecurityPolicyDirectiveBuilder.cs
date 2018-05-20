using System;

namespace Vivelin.AspNetCore.Headers
{
    public class ContentSecurityPolicyDirectiveBuilder
    {
        private const string None = "'none'";
        private const string Self = "'self'";
        private const string StrictDynamic = "'strict-dynamic'";
        private const string UnsafeEval = "'unsafe-eval'";
        private const string UnsafeInline = "'unsafe-inline'";
        private readonly ContentSecurityPolicyDirective directive;

        public ContentSecurityPolicyDirectiveBuilder(string name)
        {
            directive = new ContentSecurityPolicyDirective(name);
        }

        public static implicit operator ContentSecurityPolicyDirective(ContentSecurityPolicyDirectiveBuilder builder)
        {
            return builder.Build();
        }

        public ContentSecurityPolicyDirectiveBuilder AllowEval() => Add(UnsafeEval);

        public ContentSecurityPolicyDirectiveBuilder AllowHost(string host) => Add(host);

        public ContentSecurityPolicyDirectiveBuilder AllowInline() => Add(UnsafeInline);

        public ContentSecurityPolicyDirectiveBuilder AllowFromOrigin(string origin) => Add(origin);

        public ContentSecurityPolicyDirectiveBuilder AllowFromScheme(string scheme) => Add(scheme);

        public ContentSecurityPolicyDirectiveBuilder AllowFromSelf() => Add(Self);

        public ContentSecurityPolicyDirectiveBuilder AllowFromUrl(string url) => Add(url);

        public ContentSecurityPolicyDirectiveBuilder DisallowAll() => Add(None);

        public ContentSecurityPolicyDirectiveBuilder Dynamic() => Add(StrictDynamic);

        public ContentSecurityPolicyDirective Build()
        {
            return directive;
        }

        protected ContentSecurityPolicyDirectiveBuilder Add(string value)
        {
            if (directive.Values.Contains(None))
                throw new InvalidOperationException("Adding additional values to a directive after disallowing all requests will have no effect.");

            directive.Values.Add(value);
            return this;
        }
    }
}