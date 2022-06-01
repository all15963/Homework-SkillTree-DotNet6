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
            var pageNumber = page.DoTryGetNumber();

            // 每5筆為一分頁
            IPagedList<Articles> onePageOfArticles = _context.Articles.ToPagedList(pageNumber, 5);

            return View(onePageOfArticles);
        }

        [Route("{Controller}/{Action}/{tag}")]
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

        public async Task<IActionResult> KeywordList(string keyword)
        {
            // 找到快取物件
            byte[] objectFromCache = await _distributedCache.GetAsync(keyword);

            if (objectFromCache != null)
            {
                var jsonToDeserialize = System.Text.Encoding.UTF8.GetString(objectFromCache);
                var cachedResult = JsonConvert.DeserializeObject<IEnumerable<Articles>>(jsonToDeserialize);
                if (cachedResult != null)
                    return View(cachedResult);
            }

            IQueryable<Articles> articles = _context.Articles;

            if (string.IsNullOrWhiteSpace(keyword) == false)
                articles = articles.Where(m => m.Title.Contains(keyword) || m.Body.Contains(keyword));

            // Serialize the response
            byte[] objectToCache = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(articles.ToList());
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(5))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

            // Cache it
            await _distributedCache.SetAsync(keyword, objectToCache, cacheEntryOptions);
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