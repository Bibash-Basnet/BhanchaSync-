using System.ComponentModel.DataAnnotations;

namespace BhanchaSync.Web.Models;

// Shapes the data needed for the "take a new order" screen:
// pick a table (or Takeout), then set a quantity for any menu items
// being ordered. Items left at quantity 0 are simply not included.
public class OrderCreateViewModel
{
    public int? TableId { get; set; }

    public OrderType OrderType { get; set; } = OrderType.DineIn;

    public List<MenuItemSelectionViewModel> MenuItems { get; set; } = new();
}

public class MenuItemSelectionViewModel
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [Range(0, 50)]
    public int Quantity { get; set; }

    [StringLength(200)]
    public string? Notes { get; set; }
}
