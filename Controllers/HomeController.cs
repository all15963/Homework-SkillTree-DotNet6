using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Models;
using System.Diagnostics;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;
using X.PagedList;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Newtonsoft.Json;

namespace MVCHomework6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;
        private readonly IDistributedCache _distributedCache;


        public HomeController(ILogger<HomeController> logger, BlogDbContext context, IDistributedCache distributedCache)
        {
            _logger       = logger;
            _context = context;
            _distributedCache = distributedCache;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;

            // 每5筆為一分頁
            IPagedList<Articles> onePageOfArticles = _context.Articles.ToPagedList(pageNumber, 5);

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