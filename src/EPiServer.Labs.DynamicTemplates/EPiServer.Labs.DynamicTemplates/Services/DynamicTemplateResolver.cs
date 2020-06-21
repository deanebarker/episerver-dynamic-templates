using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System.Collections.Concurrent;
using System.Linq;

namespace DeaneBarker.DynamicTemplates.Services
{
    [ServiceConfiguration(typeof(IDynamicTemplateResolver))]
    public class DynamicTemplateResolver : IDynamicTemplateResolver
    {
        // This is the top level folder where all the templates are stored
        private static string TEMPLATE_FOLDER_NAME = "Templates";

        // This is the type name for the templates
        private static string TEMPLATE_TYPE_NAME = "TemplateBlock";

        // This is the name for the ContentArea containing the element blocks
        private static string ELEMENT_PROPERTY_NAME = "Elements";

        // This is used to cache lookups
        private static ConcurrentDictionary<string, ContentReference> templateCache;

        private IContentRepository _repo;

        static DynamicTemplateResolver()
        {
            templateCache = new ConcurrentDictionary<string,ContentReference>();
        }

        public DynamicTemplateResolver(IContentRepository repo)
        {
            _repo = repo;
        }

        // This runs whenever content is created, moved, or deleted.
        public static void PruneCache(object sender, ContentEventArgs e)
        {
            // If a template is modified, remove it from the cache, just to be safe
            if(e.Content.GetOriginalType().Name == TEMPLATE_TYPE_NAME)
            {
                templateCache.TryRemove(e.Content.Name, out ContentReference dontCare);
            }
        }

        // This locates the appropriate template for a content object
        public ContentArea GetTemplate(ContentData forContent)
        {
            var contentTypeName = forContent.Property["PageTypeName"].ToString();

            // If it's in the cache, return it
            if(templateCache.TryGetValue(contentTypeName, out ContentReference contentRef))
            {
                return GetElementsProperty(_repo.Get<IContent>(contentRef));
            }

            // Not cached, so look for it in a top-level block folder called "Templates" (by default; that could change), named for the content type name
            var topLevelFolders = _repo.GetChildren<IContent>(ContentReference.GlobalBlockFolder);
            var templatesFolder = topLevelFolders.FirstOrDefault(f => f.Name == TEMPLATE_FOLDER_NAME);
            if (templatesFolder == null)
            {
                return null;
            }
            var template = _repo.GetChildren<IContent>(templatesFolder.ContentLink).FirstOrDefault(t => t.Name == contentTypeName && t.GetOriginalType().Name == TEMPLATE_TYPE_NAME);

            if (template != null)
            {
                // Found it. Cache it...
                templateCache[contentTypeName] = template.ContentLink;

                // ...and return a ContentArea called "Elements" (by default; that could change)
                return GetElementsProperty(template);
            }

            // This is no template for this content type
            return null;
        }

        // This handles the logic of whether or not you should return a template or the local value
        public ContentArea GetTemplate(ContentArea propertyValue, ContentData contentData)
        {
            if (propertyValue != null && propertyValue.Count > 0)
            {
                // This has a local value
                return propertyValue;
            }

            var template = GetTemplate(contentData);
            if (template != null)
            {
                // We have a template
                return template;
            }

            return null;
        }

        private ContentArea GetElementsProperty(IContent contentData)
        {
            return (ContentArea)contentData.Property[ELEMENT_PROPERTY_NAME].Value;
        }
    }
}