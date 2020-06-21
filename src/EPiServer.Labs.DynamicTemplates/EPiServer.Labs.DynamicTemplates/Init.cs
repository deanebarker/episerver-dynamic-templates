using DeaneBarker.DynamicTemplates.Services;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace DeaneBarker.DynamicTemplates
{
    [InitializableModule]
    public class Init : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // We'll clear the cache whenever any TemplateBlock is modified
            var events = ServiceLocator.Current.GetInstance<IContentEvents>();
            events.DeletedContent += DynamicTemplateResolver.PruneCache;
            events.MovedContent += DynamicTemplateResolver.PruneCache;
            events.CreatedContent += DynamicTemplateResolver.PruneCache;
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}