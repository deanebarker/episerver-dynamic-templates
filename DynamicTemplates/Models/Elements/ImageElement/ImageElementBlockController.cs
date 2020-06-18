using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class ImageElementBlockController : BlockController<ImageElementBlock>
    {
        public override ActionResult Index(ImageElementBlock currentBlock)
        {
            if (!currentBlock.Show) return Content(null);

            return PartialView("~/Views/Elements/Image.cshtml", new ImageElementBlockViewModel(currentBlock));
        }
    }
}