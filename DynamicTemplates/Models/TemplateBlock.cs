using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models
{
    [ContentType(
        GUID = "67F617A4-2175-4260-915E-75EDF2B924A7",
        DisplayName = "Template",
        Description = "A container for elements, mapped to a page type.",
        GroupName = "Template Elements")]

    [ImageUrl("~/DynamicTemplates/Static/element-block.png")]
    public class TemplateBlock : BlockData
    {
        public virtual ContentArea Elements { get; set; }
    }
}