using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DynamicSections
{
    /// <summary>
    /// Registers a code block in a dynamic section.
    /// </summary>
    [HtmlTargetElement("register-block")]
    public class RegisterBlockTagHelper: TagHelper
    {
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Optional, unique key for the current block. This block will be overwritten if another
        /// block is registered using the same key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Optional, name of the dynamic section this block belongs to. By default, an empty section name is used.
        /// </summary>
        public string DynamicSection { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContents = await output.GetChildContentAsync(useCachedResult: true);
            var content = childContents.GetContent();
            ViewContext.HttpContext.RegisterBlock(DynamicSection, Key, content);
            output.SuppressOutput();
        }
    }
}