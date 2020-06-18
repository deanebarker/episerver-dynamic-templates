using DeaneBarker.DynamicTemplates.Models;
using EPiServer.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    [ContentType(
        GUID = "67F617A4-2175-4260-975E-75ADF2B924A7",
        DisplayName = "Heading Element",
        Description = "A heading with custom tag definition (default value: \"Name\")",
        GroupName = "Template Elements")]
    [ImageUrl("~/DynamicTemplates/Static/element-block.png")]
    public class HeadingElementBlock : TemplateElementBaseBlock
    {
        public virtual string Tag { get; set; }
        public virtual string Text { get; set; }
    }
}