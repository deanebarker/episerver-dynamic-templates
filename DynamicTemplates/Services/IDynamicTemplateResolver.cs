using EPiServer.Core;

namespace DeaneBarker.DynamicTemplates.Services
{
    public interface IDynamicTemplateResolver
    {
        ContentArea GetTemplate(ContentData forContent);
        ContentArea GetTemplate(ContentArea localValue, ContentData forContent);
    }
}