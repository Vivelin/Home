using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Vivelin.Home.TagHelpers
{
    public static class TagHelperExtensions
    {
        public static readonly Regex LeadingWhitespace = new Regex(@"^\s*");
        public static readonly Regex TrailingWhitespace = new Regex(@"\s*$");

        public static string GetRawContent(this TagHelperContent content, out string leadingWhitespace, out string trailingWhitespace)
        {
            var rawContent = content.GetContent(NullHtmlEncoder.Default);
            return rawContent.SplitTrim(out leadingWhitespace, out trailingWhitespace);
        }

        /// <summary>
        /// Removes all leading and trailing whitespace characters from the string.
        /// </summary>
        /// <param name="value">
        /// The string from which to remove leading and trailling whitespace characters.
        /// </param>
        /// <param name="leadingWhitespace">
        /// When this method returns, contains the whitespace that was removed
        /// from the start of the string.
        /// </param>
        /// <param name="trailingWhitespace">
        /// When this method returns, contains the whitespace that was removed
        /// from the end of the string.
        /// </param>
        /// <returns></returns>
        public static string SplitTrim(this string value, out string leadingWhitespace, out string trailingWhitespace)
        {
            var preMatch = LeadingWhitespace.Match(value);
            leadingWhitespace = preMatch.Value;

            var postMatch = TrailingWhitespace.Match(value);
            trailingWhitespace = postMatch.Value;

            return value.Substring(leadingWhitespace.Length, value.Length - leadingWhitespace.Length - trailingWhitespace.Length);
        }
    }
}