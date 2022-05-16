using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCHomework6.Areas.CreateBlog.Models;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Areas.CreateBlog.Controllers
{
    [Area("CreateBlog")]
    public class CreateController : Controller
    {
        private readonly ILogger<CreateController> _logger;
        private readonly BlogDbContext _context;
        
        public CreateController(ILogger<CreateController> logger, BlogDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var tagCloud = new List<SelectListItem>();
            foreach (var tag in _context.TagCloud.ToList())
            {
                tagCloud.Add(new SelectListItem { Value = tag.Name, Text = tag.Name });
            }
            
            var model = new CreateBlogViewModel { TagCloud = tagCloud };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(CreateBlogViewModel model)
        {
            return View();
        }
    }
}
