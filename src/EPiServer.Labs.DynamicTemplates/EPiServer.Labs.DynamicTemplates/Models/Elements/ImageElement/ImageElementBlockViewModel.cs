using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Models.Elements
{
    public class ImageElementBlockViewModel
    {
        public ImageElementBlock CurrentBlock { get; set; }

        public ImageElementBlockViewModel(ImageElementBlock currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public string ImageUrl
        {
            get
            {
                var imageId = CurrentBlock.Image ?? CurrentBlock.RenderingPageData["PageImage"] as ContentReference;
                if (imageId != null)
                {
                    var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
                    return urlResolver.GetUrl(imageId);
                }
                return null;
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImageUrl);
            }
        }
    }
}