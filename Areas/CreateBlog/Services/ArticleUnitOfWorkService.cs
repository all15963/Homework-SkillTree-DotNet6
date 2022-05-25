using Arch.EntityFrameworkCore.UnitOfWork;
using MVCHomework6.Areas.CreateBlog.Models;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Areas.CreateBlog.Services
{
    public class ArticleUnitOfWorkService : IArticleUnitOfWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Articles> _articleRepository;


        public ArticleUnitOfWorkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = unitOfWork.GetRepository<Articles>();
        }

        public async Task AddArticleAsync(CreateBlogViewModel model, string uploadPath)
        {
            string filePath = string.Empty;
            var suffix = Path.GetExtension(model.CoverPhoto.FileName);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (model.CoverPhoto.Length > 0)
            {
                filePath = Path.Combine(uploadPath, fileName + suffix);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverPhoto.CopyToAsync(fileStream);
                    await fileStream.DisposeAsync();
                }
            }

            var article = new Articles
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Body = model.Body,
                CreateDate = model.CreateDate,
                CoverPhoto = $"uploads\\{fileName + suffix}",
                Tags = string.Join(",", model.Tags)
            };
            await _articleRepository.InsertAsync(article);
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
