using BhanchaSync.Web.Data;
using BhanchaSync.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Controllers;

public class TablesController : Controller
{
    private readonly ApplicationDbContext _context;

    public TablesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Tables
    public async Task<IActionResult> Index()
    {
        var tables = await _context.Tables
            .OrderBy(t => t.Number)
            .ToListAsync();

        return View(tables);
    }

    // GET: /Tables/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tables/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Number,Status")] Table table)
    {
        if (ModelState.IsValid)
        {
            _context.Add(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(table);
    }

    // GET: /Tables/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var table = await _context.Tables.FindAsync(id);
        if (table is null) return NotFound();

        return View(table);
    }

    // POST: /Tables/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Status")] Table table)
    {
        if (id != table.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(table);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tables.AnyAsync(t => t.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(table);
    }

    // GET: /Tables/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var table = await _context.Tables.FirstOrDefaultAsync(t => t.Id == id);
        if (table is null) return NotFound();

        return View(table);
    }

    // POST: /Tables/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table is not null)
        {
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
