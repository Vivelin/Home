using System;
using System.Collections.Generic;
using System.Text;

namespace Vivelin.AspNetCore.Headers
{
    /// <summary>
    /// Represents a fetch directive that controls the location from which certain
    /// resource types may be loaded.
    /// </summary>
    public sealed class ContentSecurityPolicyDirective
    {
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ContentSecurityPolicyDirective"/> class with the specified
        /// directive name.
        /// </summary>
        /// <param name="name">The name of the directive.</param>
        public ContentSecurityPolicyDirective(string name)
        {
            Name = name;
            Values = new HashSet<string>();
        }

        /// <summary>
        /// Gets the name of the fetch directive.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a collection of the values in the directive.
        /// </summary>
        public ICollection<string> Values { get; }

        /// <summary>
        /// Returns a string that represents the fetch directive in a Content
        /// Security Policy.
        /// </summary>
        /// <returns>A string that represents the fetch directive.</returns>
        public override string ToString()
        {
            if (Values.Count == 0)
                return string.Empty;

            var directive = new StringBuilder();
            directive.Append(Name);
            directive.Append(" ");
            directive.Append(string.Join(" ", Values));
            return directive.ToString();
        }
    }
}