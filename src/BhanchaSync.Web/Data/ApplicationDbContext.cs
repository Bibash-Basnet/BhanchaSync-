using BhanchaSync.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Data;

// Inherits from IdentityDbContext so we get Users, Roles, and related
// Identity tables automatically.
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    // Note: property named "Tables" (plural) rather than "Table" —
    // TABLE is a reserved SQL keyword, so this avoids any naming friction
    // in the generated database schema.
    public DbSet<Table> Tables => Set<Table>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MenuItem>()
            .HasOne(m => m.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Table numbers should be unique — no two tables sharing a number.
        builder.Entity<Table>()
            .HasIndex(t => t.Number)
            .IsUnique();
    }
}