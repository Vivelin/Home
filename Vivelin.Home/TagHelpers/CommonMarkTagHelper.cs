using CommonMark;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vivelin.Home.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(CommonMark))]
    public class CommonMarkTagHelper : TagHelper
    {
        [HtmlAttributeName("commonmark")]
        public string CommonMark { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var html = CommonMarkConverter.Convert(CommonMark);
            output.Content.SetHtmlContent(html);
        }
    }
}
