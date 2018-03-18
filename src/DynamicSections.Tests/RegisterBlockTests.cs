using Microsoft.AspNetCore.Http;
using Xunit;

namespace DynamicSections.Tests
{
    public class RegisterBlockTests
    {
        [Fact(DisplayName = "Null section names are converted into empty strings")]
        public void NullSectionsAreConvertedToEmptyStringSections()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock(dynamicSectionName: null, key: "key", content: "content");
            Assert.Equal("content", DynamicSectionsHelpers.GetDynamicSection(httpContext, string.Empty));
        }

        [Fact(DisplayName = "Null key names are converted into empty strings")]
        public void NullKeysAreConvertedToEmptyStringKeys()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", key: null, content: "content");
            Assert.Equal("content", httpContext.GetDynamicSection("section"));
        }

        [Fact(DisplayName = "Registrations in the same section with distinct keys are stored")]
        public void RegistrationsWithDistinctKeysAreStored()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", "key1", "1");
            httpContext.RegisterBlock("section", "key2", "2");
            Assert.Equal("12", httpContext.GetDynamicSection("section"));
        }

        [Fact(DisplayName="Registrations with the same key and section should overwrite")]
        public void RegistrationsShouldOverwritePreviousValuesWithTheSameKeyIfTheyAreInTheSameSection()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", "key", "1");
            httpContext.RegisterBlock("section", "key", "2");
            Assert.Equal("2", httpContext.GetDynamicSection("section"));
        }

        [Fact(DisplayName = "Registrations with the same key but distinct section should not overwrite")]
        public void RegistrationsShouldNotOverwritePreviousValuesWithTheSameKeyIfTheyAreInDistinctSections()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section1", "key", "1");
            httpContext.RegisterBlock("section2", "key", "2");
            Assert.Equal("1", httpContext.GetDynamicSection("section1"));
            Assert.Equal("2", httpContext.GetDynamicSection("section2"));
        }

        [Fact(DisplayName = "Registrations mixing equal and distinct keys are stored")]
        public void RegistrationsMixingEqualAndDistinctKeysAreStored()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RegisterBlock("section", "key1", "1");
            httpContext.RegisterBlock("section", "key1", "2");
            httpContext.RegisterBlock("section", "key2", "3");
            Assert.Equal("23", httpContext.GetDynamicSection("section"));
        }

    }
}
