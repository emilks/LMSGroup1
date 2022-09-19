using LMS.Core.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LMS.Web.TagHelpers
{
    /*
     * Tag helpers use a naming convention that targets elements of the root class name
     * (minus the TagHelper portion of the class name). In this example, the root name of
     * FileTagHelper is file, so the <file> tag will be targeted.
     */
    [HtmlTargetElement("filehelper")]
    public class FileTagHelper : TagHelper
    {
        public Document Document { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            var image = $"<img src = \"/images/file-earmark.svg\" alt=\"\" width = \"20\" height = \"20\">";
            var content = $"<span><a style=\"color: black; text-decoration: none;\" href=\"{Document.FilePath}\" download>{image} {Document.Name}</a></span>";

            output.Content.SetHtmlContent(content);
        }
    }
}
