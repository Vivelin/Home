using CommonMark;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vivelin.Home.TagHelpers
{
    [HtmlTargetElement(Attributes = "use-commonmark")]
    public class CommonMarkTagHelper : TagHelper
    {
        [HtmlAttributeName("use-commonmark")]
        public bool UseCommonMark { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (UseCommonMark)
            {
                var childContent = await output.GetChildContentAsync();
                var rawContent = childContent.GetRawContent(out string pre, out string post);

                var html = CommonMarkConverter.Convert(rawContent).Trim();
                output.Content.SetHtmlContent(pre + html + post);
            }
        }
    }
}
