using DeaneBarker.DynamicTemplates.Models;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    [ContentType(
        GUID = "67F617A4-3175-4260-975E-75EDF2B924A7",
        DisplayName = "Image Element",
        Description = "An image with defined alignment (default: \"PageImage\")",
        GroupName = "Template Elements")]
    [ImageUrl("~/EPiServer/EPiServer.Labs.DynamicTemplates/Static/element-block.png")]
    public class ImageElementBlock : TemplateElementBaseBlock
    {
        public virtual string Align { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}