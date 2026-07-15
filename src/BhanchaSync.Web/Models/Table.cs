using System.ComponentModel.DataAnnotations;

namespace BhanchaSync.Web.Models;

public enum TableStatus
{
    Available,
    Occupied,
    Reserved
}

public class Table
{
    public int Id { get; set; }

    [Required]
    [Range(1, 999, ErrorMessage = "Table number must be between 1 and 999.")]
    public int Number { get; set; }

    public TableStatus Status { get; set; } = TableStatus.Available;
}
