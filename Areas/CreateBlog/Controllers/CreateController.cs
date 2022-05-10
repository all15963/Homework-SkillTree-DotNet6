using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Areas.CreateBlog.Models;

namespace MVCHomework6.Areas.CreateBlog.Controllers
{
    [Area("CreateBlog")]
    public class CreateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateBlogViewModel model)
        {
            return View();
        }
    }
}
