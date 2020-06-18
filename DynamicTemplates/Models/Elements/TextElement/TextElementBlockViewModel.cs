using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class TextElementBlockViewModel
    {
        public TextElementBlock CurrentBlock { get; set; }

        public TextElementBlockViewModel(TextElementBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public string Text
        {
            get
            {
                var fromProperty = CurrentBlock.FromProperty ?? "MainBody";
                return CurrentBlock.RenderingPageData[fromProperty]?.ToString() ?? string.Empty;
            }
        }
    }
}