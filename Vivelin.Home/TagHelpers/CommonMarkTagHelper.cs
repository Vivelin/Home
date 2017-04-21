using CommonMark;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vivelin.Home.TagHelpers
{
    [HtmlTargetElement(Attributes = "commonmark")]
    public class CommonMarkTagHelper : TagHelper
    {
        [HtmlAttributeName("commonmark")]
        public string CommonMark { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = CommonMarkConverter.Convert(CommonMark);
            output.PreContent.SetHtmlContent(html);
        }
    }
}
