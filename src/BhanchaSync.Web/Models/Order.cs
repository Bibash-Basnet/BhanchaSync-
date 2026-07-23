using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BhanchaSync.Web.Models;

public enum OrderType
{
    DineIn,
    Takeout
}

// The order lifecycle. Each order moves forward through these stages;
// Cancelled is the only "escape hatch" and is only allowed before
// the kitchen starts preparing (enforced in the controller, not here).
public enum OrderStatus
{
    Placed,
    Preparing,
    Ready,
    Served,
    Billed,
    Cancelled
}

public class Order
{
    public int Id { get; set; }

    // Null when OrderType is Takeout — a takeout order has no table.
    public int? TableId { get; set; }
    public Table? Table { get; set; }

    public OrderType OrderType { get; set; } = OrderType.DineIn;

    public OrderStatus Status { get; set; } = OrderStatus.Placed;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // The waiter who took the order.
    [Required]
    public string WaiterId { get; set; } = string.Empty;
    public IdentityUser? Waiter { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}
