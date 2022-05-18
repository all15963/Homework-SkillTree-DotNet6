using Microsoft.AspNetCore.Mvc;

namespace MVCHomework6.Areas.GoogleSearch.Controllers
{
    [Area("GoogleSearch")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
