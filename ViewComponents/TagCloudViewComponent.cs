using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Data.Database;
using MVCHomework6.Models;

namespace MVCHomework6.ViewComponents
{
    public class TagCloudViewComponent : ViewComponent
    {
        private readonly ILogger<TagCloudViewComponent> _logger;
        private readonly BlogDbContext _context;

        public TagCloudViewComponent(ILogger<TagCloudViewComponent> logger, BlogDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var tags = new List<string>() { "SkillTree", "twMVC", "demoshop", "Dotblogs", "MVC" };
            var tagCloud = new List<TagCloudViewModel>();

            foreach (var item in tags)
            {
                var tagCloudViewModel = new TagCloudViewModel
                {
                    Tag = item,
                    Count = _context.Articles.Where(m => m.Tags.Contains(item)).Count()
                };

                tagCloud.Add(tagCloudViewModel);
            }

            return View(tagCloud);
        }
    }
}
