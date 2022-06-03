using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using MVCHomework6.Areas.CreateBlog.Services;
using MVCHomework6.Data;
using MVCHomework6.Data.Database;
using MVCHomework6.Models;

var builder = WebApplication.CreateBuilder(args);
//本範例使用 EntityFramework inMemory 沒有實體資料庫全部在記憶體內（對於測試和POC是非常好用的）
builder.Services.AddDbContext<BlogDbContext>(
    options => options.UseInMemoryDatabase("SkillTreeBlog")
    ).AddUnitOfWork<BlogDbContext>();

builder.Services.AddTransient<IArticleUnitOfWorkService, ArticleUnitOfWorkService>();
builder.Services.AddTransient<ITagCloudUnitOfWorkService, TagCloudUnitOfWorkService>();

// IActionContextAccessor DI
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
// IUrlHelperFactory DI
builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
// DistributedMemoryCache DI
builder.Services.AddDistributedMemoryCache();


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Json File
builder.Configuration.AddJsonFile("appsettings.XPagedList.json", false, true);
builder.Services.Configure<XPagedListModel>(builder.Configuration.GetSection("PageXList"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<BlogDbContext>();
        context.Database.EnsureCreated();
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
