using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BhanchaSync.Web.Data;

// Inherits from IdentityDbContext so we get Users, Roles, and related
// Identity tables automatically. We'll add our own entities (MenuItem,
// Table, Order, etc.) here in the coming days.
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Custom entity configuration will go here as we add
        // Category, MenuItem, Table, Order, etc. in later days.
    }
}
