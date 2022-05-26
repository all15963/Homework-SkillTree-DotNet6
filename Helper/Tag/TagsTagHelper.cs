using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCHomework6.Helper.Tag
{
    public class TagsTagHelper : TagHelper
    {
        private IUrlHelperFactory UrlHelperFactory { get; }
        private IActionContextAccessor Accessor { get; }
        public TagsTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor accessor)
        {
            UrlHelperFactory = urlHelperFactory;
            Accessor = accessor;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var actionContext = Accessor.ActionContext;
            var urlHelper = UrlHelperFactory.GetUrlHelper(actionContext);
            var controller = urlHelper.ActionContext.RouteData.Values["controller"];
            
            var childContent = await output.GetChildContentAsync();
            var content = childContent.GetContent();

            var href = urlHelper.Action(new UrlActionContext
            {
                Action = "TagList",
                Controller = (string)controller,
                Values = new { tag = content }
            });
            output.TagName = "a";
            output.Attributes.SetAttribute("href", href);
            output.Content.SetContent(content);

            //return base.ProcessAsync(context, output);
        }
    }
}
