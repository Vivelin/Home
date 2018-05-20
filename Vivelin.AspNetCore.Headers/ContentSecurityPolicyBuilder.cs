using System;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Provides an API for configuring a Content Security Policy.
    /// </summary>
    public class ContentSecurityPolicyBuilder
    {
        /// <summary>
        /// Controls requests which will populate a frame (e.g.
        /// <c>&lt;iframe&gt;</c> and <c>&lt;frame&gt;</c>) or a worker.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder ChildSrc { get; }
            = new ContentSecurityPolicyDirectiveBuilder("child-src");

        /// <summary>
        /// Restricts the URLs which can be loaded using script interfaces. This
        /// includes APIs like <c>fetch()</c>, XHR, <c>&lt;a&gt;</c>’s <c>ping</c>
        /// as well as WebSocket connections.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Fetch { get; }
            = new ContentSecurityPolicyDirectiveBuilder("connect-src");

        /// <summary>
        /// Controls requests for which no other directives are specified.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Default { get; }
            = new ContentSecurityPolicyDirectiveBuilder("default-src");

        /// <summary>
        /// Restricts the URLs from which font resources may be loaded.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Fonts { get; }
            = new ContentSecurityPolicyDirectiveBuilder("font-src");

        /// <summary>
        /// Restricts the URLs which may be loaded into nested browsing contexts.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Frames { get; }
            = new ContentSecurityPolicyDirectiveBuilder("frame-src");

        /// <summary>
        /// Restricts the URLs from which image resources may be loaded.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Images { get; }
            = new ContentSecurityPolicyDirectiveBuilder("img-src");

        /// <summary>
        /// Restricts the URLs from which application manifects may be loaded.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Manifests { get; }
            = new ContentSecurityPolicyDirectiveBuilder("manifest-src");

        /// <summary>
        /// Restricts the URLs from which video, audio and associated resources
        /// may be loaded.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Media { get; }
            = new ContentSecurityPolicyDirectiveBuilder("media-src");

        /// <summary>
        /// Restricts the URLs from which plugin content may be loaded.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Objects { get; }
            = new ContentSecurityPolicyDirectiveBuilder("object-src");

        /// <summary>
        /// Restricts the URLs from which scripts may be executed.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Scripts { get; }
            = new ContentSecurityPolicyDirectiveBuilder("script-src");

        /// <summary>
        /// Restricts the locations from which style may be applied to a document.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Styles { get; }
            = new ContentSecurityPolicyDirectiveBuilder("style-src");

        /// <summary>
        /// Restricts the URLs which may be loaded as a worker, shared worker or
        /// service worker.
        /// </summary>
        public ContentSecurityPolicyDirectiveBuilder Workers { get; }
            = new ContentSecurityPolicyDirectiveBuilder("worker-src");

        /// <summary>
        /// Returns the directives constructed by the specified builder.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="ContentSecurityPolicyBuilder"/> whose directives to return.
        /// </param>
        public static implicit operator ContentSecurityPolicyDirective[] (ContentSecurityPolicyBuilder builder)
        {
            return builder.Build();
        }

        /// <summary>
        /// Returns a collection of the directives constructed by the <see cref="ContentSecurityPolicyBuilder"/>.
        /// </summary>
        /// <returns>
        /// An array of <see cref="ContentSecurityPolicyDirective"/> objects.
        /// </returns>
        protected internal ContentSecurityPolicyDirective[] Build()
        {
            return new ContentSecurityPolicyDirective[]
            {
                ChildSrc,
                Fetch,
                Default,
                Fonts,
                Frames,
                Images,
                Manifests,
                Media,
                Objects,
                Scripts,
                Styles,
                Workers
            };
        }
    }
}