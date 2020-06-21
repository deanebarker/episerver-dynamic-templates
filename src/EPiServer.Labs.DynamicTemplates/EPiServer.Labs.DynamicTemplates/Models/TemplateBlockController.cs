using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace DeaneBarker.DynamicTemplates.Models
{ 
    public class TemplateBlockController : BlockController<TemplateBlock>
    {
        public override ActionResult Index(TemplateBlock currentBlock)
        {
            return PartialView("~/Views/Elements/Template.cshtml", currentBlock);
        }
    }
}