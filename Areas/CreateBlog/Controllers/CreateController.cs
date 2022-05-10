using Microsoft.AspNetCore.Mvc;

namespace MVCHomework6.Areas.CreateBlog.Controllers
{
    [Area("CreateBlog")]
    public class CreateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
