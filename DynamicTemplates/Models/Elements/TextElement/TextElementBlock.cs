using DeaneBarker.DynamicTemplates.Models;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    [ContentType(
        GUID = "67F617A4-2175-4260-975E-75EDF2B924A7",
        DisplayName = "Text Element",
        Description = "A body of text (default value: \"MainBody\")",
        GroupName = "Template Elements")]
    [ImageUrl("~/DynamicTemplates/Static/element-block.png")]
    public class TextElementBlock : TemplateElementBaseBlock
    {
        public virtual XhtmlString MainBody { get; set; }

        public virtual string FromProperty { get; set; }
    }
}