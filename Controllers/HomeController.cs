using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Models;
using System.Diagnostics;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;
using X.PagedList;

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
        public IActionResult Index(string tag, int? page, string? keyword)
        {
            var pageNumber = page ?? 1;
            var articles = (IQueryable<Articles>)_context.Articles;

            if (string.IsNullOrWhiteSpace(tag) == false)
                articles = articles.Where(m => m.Tags.Contains(tag));

            if (string.IsNullOrWhiteSpace(keyword) == false)
                articles = articles.Where(m => m.Title.Contains(keyword) || m.Body.Contains(keyword));

            // 每5筆為一分頁
            var onePageOfArticles = articles.ToPagedList(pageNumber, 5);

            return View(onePageOfArticles);
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