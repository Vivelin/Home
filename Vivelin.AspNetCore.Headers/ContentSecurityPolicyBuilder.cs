using System;

namespace Vivelin.AspNetCore.Headers
{
    public class ContentSecurityPolicyBuilder
    {
        public ContentSecurityPolicyDirectiveBuilder ChildSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("child-src");
        public ContentSecurityPolicyDirectiveBuilder ConnectSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("connect-src");
        public ContentSecurityPolicyDirectiveBuilder DefaultSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("default-src");
        public ContentSecurityPolicyDirectiveBuilder FontSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("font-src");
        public ContentSecurityPolicyDirectiveBuilder FrameSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("frame-src");
        public ContentSecurityPolicyDirectiveBuilder ImageSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("img-src");
        public ContentSecurityPolicyDirectiveBuilder ManifectSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("manifest-src");
        public ContentSecurityPolicyDirectiveBuilder MediaSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("media-src");
        public ContentSecurityPolicyDirectiveBuilder ObjectSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("object-src");
        public ContentSecurityPolicyDirectiveBuilder ScriptSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("script-src");
        public ContentSecurityPolicyDirectiveBuilder StyleSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("style-src");
        public ContentSecurityPolicyDirectiveBuilder WorkerSrc { get; } = new ContentSecurityPolicyDirectiveBuilder("worker-src");

        public static implicit operator ContentSecurityPolicyDirective[](ContentSecurityPolicyBuilder builder)
        {
            return builder.Build();
        }

        protected internal ContentSecurityPolicyDirective[] Build()
        {
            return new ContentSecurityPolicyDirective[]
            {
                ChildSrc,
                ConnectSrc,
                DefaultSrc,
                FontSrc,
                FrameSrc,
                ImageSrc,
                ManifectSrc,
                MediaSrc,
                ObjectSrc,
                ScriptSrc,
                StyleSrc,
                WorkerSrc
            };
        }
    }
}