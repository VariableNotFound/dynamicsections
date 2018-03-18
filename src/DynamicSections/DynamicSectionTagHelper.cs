using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DynamicSections
{
    /// <summary>
    /// Renders the content of the specified dynamic section. 
    /// </summary>
    [HtmlTargetElement("dynamic-section", Attributes="Name")]
    public class DynamicSectionTagHelper : TagHelper
    {
        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Name of the dynamic section to be rendered, or "*" to render all sections.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines if the section must be removed from the memory once it has been rendered. Defaults to true.
        /// </summary>
        public bool Remove { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            output.Content.SetHtmlContent(ViewContext.HttpContext.GetDynamicSection(Name, Remove));
        }

    }
}
