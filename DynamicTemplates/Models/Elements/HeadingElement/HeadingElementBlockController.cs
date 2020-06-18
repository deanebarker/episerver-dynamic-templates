using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class HeadingElementBlockController : BlockController<HeadingElementBlock>
    {
        public override ActionResult Index(HeadingElementBlock currentBlock)
        {
            if(!currentBlock.Show) return Content(null);

            return PartialView("~/Views/Elements/Heading.cshtml", new HeadingElementBlockViewModel(currentBlock));
        }
    }

   
}