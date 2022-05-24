using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCHomework6.Areas.CreateBlog.Services
{
    public interface ITagCloudUnitOfWorkService
    {
        List<SelectListItem> GetTagCloud();
        Task SaveAsync();
    }
}
