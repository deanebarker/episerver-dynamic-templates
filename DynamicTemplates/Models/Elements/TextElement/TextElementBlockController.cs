using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class TextElementBlockController : BlockController<TextElementBlock>
    {
        public override ActionResult Index(TextElementBlock currentBlock)
        {
            if (!currentBlock.Show) return Content(null);

            return PartialView("~/Views/Elements/Text.cshtml", new TextElementBlockViewModel(currentBlock));
        }
    }

   
}