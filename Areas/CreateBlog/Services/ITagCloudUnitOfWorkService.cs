using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCHomework6.Areas.CreateBlog.Services
{
    public interface ITagCloudUnitOfWorkService
    {
        List<SelectListItem> GetTagCloud();
        void UpdateAmount(IEnumerable<string> tags);
        Task SaveAsync();
    }
}
