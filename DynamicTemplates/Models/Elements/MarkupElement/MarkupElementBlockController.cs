using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class MarkupElementBlockController : BlockController<MarkupElementBlock>
    {
        public override ActionResult Index(MarkupElementBlock currentBlock)
        {
            if (!currentBlock.Show) return Content(null);

            return Content(currentBlock.Process(currentBlock.Markup ?? string.Empty));
        }
    }
}