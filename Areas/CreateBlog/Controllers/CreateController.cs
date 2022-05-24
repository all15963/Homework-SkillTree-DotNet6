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
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        public CreateController(ILogger<CreateController> logger, BlogDbContext context, IWebHostEnvironment environment, ITagCloudUnitOfWorkService tagCloudService)
        {
            _logger = logger;
            _context = context;
            _tagCloudService = tagCloudService;
            _webHostEnvironment = environment;
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
                string filePath = string.Empty;
                var suffix = Path.GetExtension(model.CoverPhoto.FileName);
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                if (model.CoverPhoto.Length > 0)
                {
                    filePath = Path.Combine(uploads, fileName + suffix);
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
                
                _context.Add(article);
                await _context.SaveChangesAsync();
                ViewData["msg"] = "新增成功";
            }

            return View(model);
        }
    }
}
