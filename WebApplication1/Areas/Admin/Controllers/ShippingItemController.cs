using System.Threading.Tasks;
using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Admin.Controllers;
[Area("Admin")]
public class ShippingItemController : Controller
{
	private AppDbContext _context;
	public ShippingItemController(AppDbContext context)
	{
		_context = context;
	}
	public IActionResult Index()
	{
		return View(_context.ShippingItems);
	}
    public async Task<IActionResult> Detail(int id)
    {
        var model = await _context.ShippingItems.FindAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShippingItem item)
    {
        if(!ModelState.IsValid) return View();
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int id)
    {
        return Content(id.ToString());
    }
    public IActionResult Delete(int id)
    {
        return Content(id.ToString());
    }
}
