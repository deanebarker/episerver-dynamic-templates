using DeaneBarker.DynamicTemplates.Fluid;
using DeaneBarker.DynamicTemplates.Fluid.Services;
using EPiServer;
using EPiServer.Core;
using EPiServer.Data;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Fluid;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeaneBarker.DynamicTemplates.Models
{
    public class TemplateElementBaseBlock : BlockData
    {
        public ContentReference RenderingPageLink
        {
            get
            {
                var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
                return new ContentReference(pageRouteHelper.PageLink.ID);
            }
        }

        public ContentData RenderingPageData
        {
            get
            {
                var repo = ServiceLocator.Current.GetInstance<IContentRepository>();
                return repo.Get<ContentData>(RenderingPageLink);
            }
        }

        public bool IsBoilerplate
        {
            get
            {
                return EPiServer.Editor.PageEditing.PageIsInEditMode && RenderingPageLink == ContentReference.StartPage;
            }
        }

        public bool Show
        {
            get
            {
                if(string.IsNullOrWhiteSpace(ShowIf))
                {
                    return true;
                }

                var templatingService = ServiceLocator.Current.GetInstance<ITemplatingService>();
                return templatingService.EvalExpression(ShowIf, null, RenderingPageData);
            }
        }

        public string Process(string input)
        {
            var templatingService = ServiceLocator.Current.GetInstance<ITemplatingService>();

            // Should we process this at all?
            if (!IsBoilerplate && string.IsNullOrWhiteSpace(ShowIf) && !templatingService.IsTemplate(input))
            {
                // Has no templating code...
                return input;
            }

            // Process...
            return templatingService.Render(input, null, RenderingPageData);
        }

        [Display(
            GroupName = "Template",
            Order = 100)]
        public virtual string ClassName { get; set; }

        [Display(
            GroupName = "Template",
            Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string StyleRules { get; set; }

        public string StyleOutput
        {
            get
            {
                if(string.IsNullOrWhiteSpace(StyleRules))
                {
                    return string.Empty;
                }
                return StyleRules.Replace(Environment.NewLine, string.Empty);
            }
        }

        [Display(
            GroupName = "Template",
            Order = 300)]
        [UIHint(UIHint.Textarea)]
        public virtual string BoilerplateOutput { get; set; }
        
        [Display(
            GroupName = "Template",
            Order = 300)]
        public virtual string ShowIf { get; set; }

    }
}