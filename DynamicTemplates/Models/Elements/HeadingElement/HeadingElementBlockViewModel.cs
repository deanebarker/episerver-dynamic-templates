using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class HeadingElementBlockViewModel
    {
        public HeadingElementBlock CurrentBlock { get; set; }

        public HeadingElementBlockViewModel(HeadingElementBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public string Text
        {
            get
            {
                return CurrentBlock.Process(CurrentBlock.Text ?? CurrentBlock.RenderingPageData.Property["PageName"].ToString());
            }
        }
    }
}