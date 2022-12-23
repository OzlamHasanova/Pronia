using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Admin.ViewModels.Slider;

namespace WebApplication1.Areas.Admin.Controllers;
[Area("Admin")]
public class SlideItemController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public SlideItemController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        return View(_context.SlideItems);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var model = await _context.SlideItems.FindAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public IActionResult Create(int id)
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SlideCreateVM item)
    {
        if (!ModelState.IsValid) return View(item);
        if (item.Photo == null)
        {
            ModelState.AddModelError("Photo", "Please, select the photo");
            return View(item);
        }
        if (item.Photo.Length / 1024 > 100)
        {
            ModelState.AddModelError("Photo", "The Photo size must be less than 100 kbyte");
            return View(item);
        }
        if (!item.Photo.ContentType.Contains("image/"))
        {
            ModelState.AddModelError("Photo", "Please, choose image file");
            return View(item);
        }
        var wwwroot = _env.WebRootPath;
        var fileName=Guid.NewGuid().ToString()+item.Photo.FileName;
        var resultPath=Path.Combine(wwwroot, "assets", "images", "website-images", fileName);
        using(FileStream stream=new(resultPath, FileMode.Create))
        {
            await item.Photo.CopyToAsync(stream);
        }

        SlideItem slideItem = new()
        {
            Title = item.Title,
            Offer = item.Offer,
            Description=item.Description,
            Photo=fileName,
        };
        await _context.SlideItems.AddAsync(slideItem);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
