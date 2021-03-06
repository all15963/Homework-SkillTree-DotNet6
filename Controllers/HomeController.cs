using Microsoft.AspNetCore.Mvc;
using MVCHomework6.Models;
using System.Diagnostics;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;
using X.PagedList;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Newtonsoft.Json;
using MVCHomework6.Extensions;
using Microsoft.Extensions.Options;

namespace MVCHomework6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;
        private readonly IDistributedCache _distributedCache;
        private readonly XPagedListModel _xPagedListModel;


        public HomeController(ILogger<HomeController> logger, BlogDbContext context, IDistributedCache distributedCache, IOptions<XPagedListModel> xPagedListModel)
        {
            _logger = logger;
            _context = context;
            _distributedCache = distributedCache;
            _xPagedListModel = xPagedListModel.Value;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page.DoTryGetNumber();
            // 每5筆為一分頁
            IPagedList<Articles> onePageOfArticles = _context.Articles.ToPagedList(pageNumber, _xPagedListModel.pageSize);

            return View(onePageOfArticles);
        }

        [Route("~/{tag}")]
        public async Task<IActionResult> TagList(string tag)
        {
            // 找到快取物件
            byte[] objectFromCache = await _distributedCache.GetAsync(tag);

            if (objectFromCache != null)
            {
                var jsonToDeserialize = System.Text.Encoding.UTF8.GetString(objectFromCache);
                var cachedResult = JsonConvert.DeserializeObject<ICollection<Articles>>(jsonToDeserialize);
                if (cachedResult != null)
                    return View(cachedResult);
            }

            IQueryable<Articles> articles = _context.Articles;

            if (string.IsNullOrWhiteSpace(tag) == false)
                articles = articles.Where(m => m.Tags.Contains(tag));

            // Serialize the response
            byte[] objectToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(articles.ToList());
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(5))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

            // Cache it
            await _distributedCache.SetAsync(tag, objectToCache, cacheEntryOptions);
            return View(articles.ToList());
        }

        [ResponseCache(Duration = 66)]
        public IActionResult KeywordList(string keyword)
        {
            IQueryable<Articles> articles = _context.Articles;

            if (string.IsNullOrWhiteSpace(keyword) == false)
                articles = articles.Where(m => m.Title.Contains(keyword) || m.Body.Contains(keyword));

            return View(articles.ToList());
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