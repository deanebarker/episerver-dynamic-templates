using DeaneBarker.DynamicTemplates.Models;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    [ContentType(
        GUID = "67F617A4-2175-4261-975E-75EDF2B324A7",
        DisplayName = "Markup Element",
        Description = "A body of markup",
        GroupName = "Template Elements")]
    [ImageUrl("~/DynamicTemplates/Static/element-block.png")]
    public class MarkupElementBlock : TemplateElementBaseBlock
    {
        [UIHint(UIHint.Textarea)]
        public virtual string Markup { get; set; }
    }
}