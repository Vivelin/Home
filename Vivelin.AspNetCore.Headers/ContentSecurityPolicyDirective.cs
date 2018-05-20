using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vivelin.AspNetCore.Headers
{
    public sealed class ContentSecurityPolicyDirective
    {
        public ContentSecurityPolicyDirective(string name)
        {
            Name = name;
            Values = new HashSet<string>();
        }

        public string Name { get; }

        public ICollection<string> Values { get; }

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
