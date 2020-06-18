using EPiServer.Core;
using Fluid;
using log4net.ObjectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Fluid.Services
{
    public interface ITemplatingService
    {
        string Render(string template, TemplateContext context, ContentData contentData = null);
        bool EvalExpression(string expression, TemplateContext context, ContentData contentData = null);
        bool IsTemplate(string input);
        TemplateContext PopulateContextFromContentData(TemplateContext context, ContentData contentData);
    }
}