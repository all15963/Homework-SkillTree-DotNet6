using MVCHomework6.Areas.CreateBlog.Models;

namespace MVCHomework6.Areas.CreateBlog.Services
{
    public interface IArticleUnitOfWorkService
    {
        Task AddArticleAsync(CreateBlogViewModel model, string uploadPath);

        Task SaveAsync();
    }
}
