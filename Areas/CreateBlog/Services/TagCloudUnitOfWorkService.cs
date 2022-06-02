using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Areas.CreateBlog.Services
{
    public class TagCloudUnitOfWorkService : ITagCloudUnitOfWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlogDbContext _blogContext;
        private readonly IRepository<TagCloud> _tagCloudRepository;

        public TagCloudUnitOfWorkService(IUnitOfWork unitOfWork, BlogDbContext blogContext)
        {
            _unitOfWork = unitOfWork;
            _tagCloudRepository = unitOfWork.GetRepository<TagCloud>();
            _blogContext = blogContext;
        }

        /// <summary>
        /// 取得TagCloud SelectListItem
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetTagCloud()
        {
            var tagCloud = new List<SelectListItem>();
            foreach (var tag in _blogContext.TagCloud.ToList())
            {
                tagCloud.Add(new SelectListItem { Value = tag.Name, Text = tag.Name });
            }

            return tagCloud;
        }

        public void UpdateAmount(IEnumerable<string> tags)
        {
            var tagClouds = _blogContext.TagCloud.Where(m => tags.Contains(m.Name)).ToList();
            foreach (var tagCloud in tagClouds)
            {
                tagCloud.Amount += 1;
            }
            _tagCloudRepository.Update(tagClouds);
        }

        public void AddNewTag(IEnumerable<string> tags)
        {
            var tagClouds = _blogContext.TagCloud.Select(m => m.Name).ToList();
            var newTagClouds = tags.Where(m => !tagClouds.Contains(m)).ToList();
            var modelList = new List<TagCloud>();
            foreach (var item in newTagClouds)
            {
                var tag = new TagCloud
                {
                    Id = Guid.NewGuid(),
                    Name = item,
                    Amount = 1
                };
                modelList.Add(tag);
            }

            _tagCloudRepository.Insert(modelList);
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
