using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BhanchaSync.Web.Models;

public class MenuItem
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Foreign key + navigation property back to Category.
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
