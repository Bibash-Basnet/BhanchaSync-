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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // A category can have many menu items; deleting a category
        // should not silently delete its items, so we restrict that.
        builder.Entity<MenuItem>()
            .HasOne(m => m.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(m => m.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
