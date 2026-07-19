using BhanchaSync.Web.Data;
using BhanchaSync.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Controllers;

public class MenuItemsController : Controller
{
    private readonly ApplicationDbContext _context;

    public MenuItemsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /MenuItems  — visible to anyone logged in (Admin, Waiter, Kitchen)
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var menuItems = await _context.MenuItems
            .Include(m => m.Category)
            .OrderBy(m => m.Category!.Name)
            .ThenBy(m => m.Name)
            .ToListAsync();

        return View(menuItems);
    }

    // GET: /MenuItems/Create — Admin only
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name");
        return View();
    }

    // POST: /MenuItems/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Price,ImageUrl,IsAvailable,CategoryId")] MenuItem menuItem)
    {
        if (ModelState.IsValid)
        {
            _context.Add(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name", menuItem.CategoryId);
        return View(menuItem);
    }

    // GET: /MenuItems/Edit/5 — Admin only
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem is null) return NotFound();

        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name", menuItem.CategoryId);
        return View(menuItem);
    }

    // POST: /MenuItems/Edit/5
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageUrl,IsAvailable,CategoryId")] MenuItem menuItem)
    {
        if (id != menuItem.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(menuItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.MenuItems.AnyAsync(m => m.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name), "Id", "Name", menuItem.CategoryId);
        return View(menuItem);
    }

    // GET: /MenuItems/Delete/5 — Admin only
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var menuItem = await _context.MenuItems
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (menuItem is null) return NotFound();

        return View(menuItem);
    }

    // POST: /MenuItems/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var menuItem = await _context.MenuItems.FindAsync(id);
        if (menuItem is not null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}