using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DynamicSections.Tests
{
    [Trait("Category", "GetSection")]
    public class GetSectionTests
    {
        [Fact(DisplayName = "The same section can't be obtained twice")]
        public void CantGetTheSameSectionTwice()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", "key", "1");
            Assert.Equal("1", httpContext.GetDynamicSection("section"));
            Assert.Equal(string.Empty, httpContext.GetDynamicSection("section"));
        }

                
        [Fact(DisplayName = "Sections are not removed when required")]
        public void SectionsAreNotRemoved()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", "key", "1");
            Assert.Equal("1", httpContext.GetDynamicSection("*", remove: false));
            Assert.Equal("1", httpContext.GetDynamicSection("*", remove: true));
            Assert.Equal(string.Empty, httpContext.GetDynamicSection("section"));
        }

        [Fact(DisplayName = "If a section has no blocks, an empty string is returned")]
        public void EmptyStringIsReturnedIfNoBlockIsRegistered()
        {
            var httpContext = new DefaultHttpContext();
            Assert.Equal(string.Empty, httpContext.GetDynamicSection("*"));
        }

        [Fact(DisplayName = "Using asterisk returns all sections")]
        public void UsingAsteriskReturnsAllSectionsContents()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section1", "key1", "0");
            httpContext.RegisterBlock("section1", "key1", "1");
            httpContext.RegisterBlock("section1", "key2", "2");
            httpContext.RegisterBlock("section2", "key3", "3");
            Assert.Equal("123", httpContext.GetDynamicSection("*"));
        }



    }
}