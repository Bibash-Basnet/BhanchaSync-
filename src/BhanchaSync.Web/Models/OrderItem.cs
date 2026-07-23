using System.ComponentModel.DataAnnotations;

namespace BhanchaSync.Web.Models;

// Tracks each individual item's progress in the kitchen, separately
// from the overall Order.Status — e.g. 2 of 5 items on an order can
// be "Ready" while the rest are still "Preparing".
public enum OrderItemStatus
{
    Placed,
    Preparing,
    Ready
}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    [Range(1, 50)]
    public int Quantity { get; set; } = 1;

    [StringLength(200)]
    public string? Notes { get; set; }

    public OrderItemStatus Status { get; set; } = OrderItemStatus.Placed;
}
