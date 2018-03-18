using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicSections
{
    public static class DynamicSectionsHtmlExtensions
    {

        /// <summary>
        /// Registers a code block in the default dynamic section.
        /// </summary>
        /// <param name="helper">IHtmlHelper instance.</param>
        /// <param name="key">Key name. If it exists, the previous block will be overwritten.</param>
        /// <param name="content">Content to be registered.</param>
        public static string RegisterBlock(this IHtmlHelper helper, string key, string content)
        {
            return RegisterBlock(helper, null, key, content);
        }

        /// <summary>
        /// Registers a code block in a dynamic section.
        /// </summary>
        /// <param name="helper">IHtmlHelper instance.</param>
        /// <param name="dynamicSectionName">Name of the dynamic section where this block will be registered.</param>
        /// <param name="key">Key name. If it exists, the previous block will be overwritten.</param>
        /// <param name="content">Content to be registered.</param>
        public static string RegisterBlock(this IHtmlHelper helper,
            string dynamicSectionName, string key, string content)
        {
            helper.ViewContext.HttpContext
                .RegisterBlock(dynamicSectionName, key, content);
            return string.Empty;
        }

        /// <summary>
        /// Gets the content of the specified dynamic section. 
        /// </summary>
        /// <param name="helper">IHtmlHelper instance.</param>
        /// <param name="dynamicSectionName">Name of the dynamic section to get. Use "*" to get all sections contents.</param>
        /// <param name="remove">Determines if the section must be removed from the memory once it has been read. Defaults to true.</param>
        /// <returns>The section content, as a IHtmlContent.</returns>
        public static IHtmlContent DynamicSection(this IHtmlHelper helper, string dynamicSectionName = "*", bool remove = true)
        {
            var blocks = helper.ViewContext.HttpContext
                .GetDynamicSection(dynamicSectionName, remove);
            return new HtmlString(blocks);
        }
    }
}