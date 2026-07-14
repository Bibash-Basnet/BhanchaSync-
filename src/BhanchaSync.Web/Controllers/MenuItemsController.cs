using BhanchaSync.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Controllers;

public class MenuItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public MenuItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /MenuItems
    public async Task<IActionResult> Index()
    {
        var menuItems = await _context.MenuItems
            .Include(m => m.Category)   // eager-load Category so the view can show its name
            .OrderBy(m => m.Category!.Name)
            .ThenBy(m => m.Name)
            .ToListAsync();

        return View(menuItems);
    }
}
