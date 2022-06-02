using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCHomework6.Areas.CreateBlog.Models;
using MVCHomework6.Areas.CreateBlog.Services;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Areas.CreateBlog.Controllers
{
    [Area("CreateBlog")]
    public class CreateController : Controller
    {
        private readonly ILogger<CreateController> _logger;
        private readonly BlogDbContext _context;
        private readonly ITagCloudUnitOfWorkService _tagCloudService;
        private readonly IArticleUnitOfWorkService _articleService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CreateController(ILogger<CreateController> logger, BlogDbContext context,
            IWebHostEnvironment environment, ITagCloudUnitOfWorkService tagCloudService, 
            IArticleUnitOfWorkService articleService)
        {
            _logger = logger;
            _context = context;
            _tagCloudService = tagCloudService;
            _webHostEnvironment = environment;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            var model = new CreateBlogViewModel 
            { 
                TagCloud = _tagCloudService.GetTagCloud()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(CreateBlogViewModel model)
        {
            model.TagCloud = _tagCloudService.GetTagCloud();

            if (ModelState.IsValid)
            {
                string uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                await _articleService.AddArticleAsync(model, uploads);

                _tagCloudService.AddNewTag(model.Tags);
                _tagCloudService.UpdateAmount(model.Tags);

                await _tagCloudService.SaveAsync();
                ViewData["msg"] = "新增成功";
            }

            return View(model);
        }
    }
}
