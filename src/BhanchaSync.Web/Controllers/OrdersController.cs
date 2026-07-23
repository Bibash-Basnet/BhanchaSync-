using BhanchaSync.Web.Data;
using BhanchaSync.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Orders — active orders board, visible to any logged-in role
    public async Task<IActionResult> Index()
    {
        var activeOrders = await _context.Orders
            .Include(o => o.Table)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem)
            .Where(o => o.Status != OrderStatus.Billed && o.Status != OrderStatus.Cancelled)
            .OrderBy(o => o.CreatedAt)
            .ToListAsync();

        return View(activeOrders);
    }

    // GET: /Orders/Create — Waiter or Admin only
    [Authorize(Roles = "Admin,Waiter")]
    public async Task<IActionResult> Create()
    {
        var vm = new OrderCreateViewModel
        {
            MenuItems = await _context.MenuItems
                .Where(m => m.IsAvailable)
                .OrderBy(m => m.Category!.Name).ThenBy(m => m.Name)
                .Select(m => new MenuItemSelectionViewModel
                {
                    MenuItemId = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    Quantity = 0
                })
                .ToListAsync()
        };

        await PopulateTablesAsync();
        return View(vm);
    }

    // POST: /Orders/Create
    [HttpPost]
    [Authorize(Roles = "Admin,Waiter")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderCreateViewModel vm)
    {
        var selectedItems = vm.MenuItems.Where(m => m.Quantity > 0).ToList();

        if (vm.OrderType == OrderType.DineIn && vm.TableId is null)
        {
            ModelState.AddModelError(string.Empty, "Please select a table for a dine-in order.");
        }

        if (!selectedItems.Any())
        {
            ModelState.AddModelError(string.Empty, "Add at least one item to the order.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateTablesAsync();
            return View(vm);
        }

        var order = new Order
        {
            TableId = vm.OrderType == OrderType.DineIn ? vm.TableId : null,
            OrderType = vm.OrderType,
            Status = OrderStatus.Placed,
            WaiterId = _userManager.GetUserId(User)!,
            OrderItems = selectedItems.Select(i => new OrderItem
            {
                MenuItemId = i.MenuItemId,
                Quantity = i.Quantity,
                Notes = i.Notes,
                Status = OrderItemStatus.Placed
            }).ToList()
        };

        _context.Orders.Add(order);

        if (order.OrderType == OrderType.DineIn && order.TableId is not null)
        {
            var table = await _context.Tables.FindAsync(order.TableId);
            if (table is not null) table.Status = TableStatus.Occupied;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateTablesAsync()
    {
        ViewBag.Tables = await _context.Tables
            .Where(t => t.Status == TableStatus.Available)
            .OrderBy(t => t.Number)
            .ToListAsync();
    }
}
