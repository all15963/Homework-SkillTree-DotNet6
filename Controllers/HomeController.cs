using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Models;
using System.Diagnostics;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;

namespace MVCHomework6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;


        public HomeController(ILogger<HomeController> logger, BlogDbContext context)
        {
            _logger       = logger;
            _context = context;
        }

        [Route("{tag?}")]
        public IActionResult Index(string tag)
        {
            var model = new List<Articles>();
            if (string.IsNullOrWhiteSpace(tag) == false)
                model = _context.Articles.Where(m => m.Tags.Contains(tag)).ToList();
            else
                model = _context.Articles.ToList();//這是範例，已經塞了20筆資料進去
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}