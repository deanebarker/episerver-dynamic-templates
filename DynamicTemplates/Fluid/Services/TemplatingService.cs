using EPiServer.Core;
using EPiServer.ServiceLocation;
using Fluid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Fluid.Services
{
    [ServiceConfiguration(typeof(ITemplatingService))]
    public class TemplatingService : ITemplatingService
    {
        public bool EvalExpression(string expression, TemplateContext context, ContentData contentData = null)
        {
            context = PopulateContextFromContentData(context, contentData);

            var matchString = "TRUE";
            var showIf = string.Concat("{% if ", expression, " %}", matchString, "{% endif %}");
            var showIfTemplate = FluidTemplate.Parse(showIf);
            return showIfTemplate.Render(context) == matchString;
        }

        public string Render(string input, TemplateContext context, ContentData contentData = null)
        {
            if (!IsTemplate(input))
            {
                // Has no templating code...
                return input;
            }

            context = PopulateContextFromContentData(context, contentData);

            var template = FluidTemplate.Parse(input);
            return template.Render(context);
        }

        public TemplateContext PopulateContextFromContentData(TemplateContext context, ContentData contentData)
        {
            context = context ?? new TemplateContext();
            if (contentData != null)
            {
                foreach (var prop in contentData.Property)
                {
                    context.SetValue(prop.Name, prop.Value);
                }
            }
            return context;
        }

        public bool IsTemplate(string input)
        {
            return input.Contains("{{") || input.Contains("{%");
        }
    }
}