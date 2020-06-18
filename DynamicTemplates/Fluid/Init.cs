using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Fluid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeaneBarker.DynamicTemplates.Fluid
{
    [InitializableModule]
    public class Init : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            TemplateContext.GlobalFilters.AddFilter("lower", FluidFilters.Lower);
            TemplateContext.GlobalFilters.AddFilter("upper", FluidFilters.Upper);
            TemplateContext.GlobalFilters.AddFilter("format", FluidFilters.Format);
            TemplateContext.GlobalFilters.AddFilter("days_ago", FluidFilters.DaysAgo);
        }

        public void Uninitialize(InitializationEngine context)
        {
           
        }
    }
}