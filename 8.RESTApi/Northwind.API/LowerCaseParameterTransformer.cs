using Microsoft.AspNetCore.Routing;

namespace CatalogService.API
{
    public class LowerCaseParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value?.ToString().ToLower();
        }
    }
}
