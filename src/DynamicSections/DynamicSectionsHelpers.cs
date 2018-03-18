using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DynamicSections
{
    public static class DynamicSectionsHelpers
    {
        private static string HttpContextItemName = nameof(DynamicSectionsHelpers);

        /// <summary>
        /// Registers a code block in a dynamic section.
        /// </summary>
        /// <param name="httpContext">HttpContext instance.</param>
        /// <param name="dynamicSectionName">Name of the dynamic section where this block will be registered.</param>
        /// <param name="key">Key name. If it exists, the previous block will be overwritten.</param>
        /// <param name="content">Content to be registered.</param>
        public static void RegisterBlock(this HttpContext httpContext,
            string dynamicSectionName, string key, string content)
        {
            var sections = (SectionDictionary)httpContext.Items[HttpContextItemName];
            if (sections == null)
            {
                sections = new SectionDictionary();
                httpContext.Items[HttpContextItemName] = sections;
            }
            dynamicSectionName = dynamicSectionName ?? string.Empty;
            key = key ?? string.Empty;
            if (!sections.ContainsKey(dynamicSectionName))
            {
                sections[dynamicSectionName] = new BlockDictionary();
            }
            sections[dynamicSectionName][key] = content;
        }

        /// <summary>
        /// Gets the content of the specified dynamic section. 
        /// </summary>
        /// <param name="httpContext">HttpContext instance.</param>
        /// <param name="dynamicSectionName">Name of the dynamic section to get. Use "*" to get all sections contents.</param>
        /// <param name="remove">Determines if the section must be removed from the memory once it has been read. Defaults to true.</param>
        /// <returns>The section content, as a string.</returns>
        public static string GetDynamicSection(this HttpContext httpContext, 
            string dynamicSectionName, bool remove = true)
        {
            var result = string.Empty;
            var sections = (SectionDictionary)httpContext.Items[HttpContextItemName];
            if (sections == null)
                return result;

            dynamicSectionName = dynamicSectionName ?? String.Empty;
            if (dynamicSectionName == "*")
            {
                // Get all sections
                result = GetSectionAndRemoveWhenRequested(sections, remove, sections.Keys.ToArray());
            }
            else if (sections.ContainsKey(dynamicSectionName))
            {
                // Get only the specified section
                result = GetSectionAndRemoveWhenRequested(sections, remove, dynamicSectionName);
            }

            return result;
        }

        private static string GetSectionAndRemoveWhenRequested(SectionDictionary sections, 
            bool remove, params string[] sectionNames)
        {
            var sb = new StringBuilder();
            foreach (var sectionName in sectionNames)
            {
                var contents = sections[sectionName].Select(c => c.Value);
                foreach (var content in contents)
                {
                    sb.Append(content);
                }

                if (remove) // This section can be obtained only once!
                {
                    sections.Remove(sectionName); 
                }
            }
            return sb.ToString();
        }

        private class BlockDictionary : Dictionary<string, string>
        {
            public BlockDictionary() : base(StringComparer.CurrentCultureIgnoreCase) { }
        }

        private class SectionDictionary : Dictionary<string, BlockDictionary>
        {
            public SectionDictionary() : base(StringComparer.CurrentCultureIgnoreCase) { }
        }
    }
}