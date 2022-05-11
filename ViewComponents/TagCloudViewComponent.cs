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
            return View(_context.TagCloud.ToList());
        }
    }
}
