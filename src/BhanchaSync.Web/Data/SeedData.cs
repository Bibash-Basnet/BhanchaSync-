using BhanchaSync.Web.Models;

namespace BhanchaSync.Web.Data;

// Populates a few sample categories and menu items on first run,
// purely so we have real data to build/test views against.
// This will be replaced by proper Admin CRUD forms later in the week.
public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Categories.Any())
        {
            return; // already seeded
        }

        var starters = new Category { Name = "Starters" };
        var mains = new Category { Name = "Main Course" };
        var drinks = new Category { Name = "Drinks" };

        context.Categories.AddRange(starters, mains, drinks);
        context.SaveChanges();

        context.MenuItems.AddRange(
            new MenuItem
            {
                Name = "Momo (Chicken)",
                Description = "Steamed dumplings served with tomato achar",
                Price = 180,
                IsAvailable = true,
                CategoryId = starters.Id
            },
            new MenuItem
            {
                Name = "Chatamari",
                Description = "Newari rice crepe with egg and minced meat topping",
                Price = 220,
                IsAvailable = true,
                CategoryId = starters.Id
            },
            new MenuItem
            {
                Name = "Dal Bhat Set",
                Description = "Rice, lentils, seasonal vegetables, and pickle",
                Price = 320,
                IsAvailable = true,
                CategoryId = mains.Id
            },
            new MenuItem
            {
                Name = "Chicken Sekuwa",
                Description = "Grilled marinated chicken skewers",
                Price = 380,
                IsAvailable = true,
                CategoryId = mains.Id
            },
            new MenuItem
            {
                Name = "Masala Chiya",
                Description = "Spiced Nepali milk tea",
                Price = 60,
                IsAvailable = true,
                CategoryId = drinks.Id
            }
        );

        context.SaveChanges();
    }
}
