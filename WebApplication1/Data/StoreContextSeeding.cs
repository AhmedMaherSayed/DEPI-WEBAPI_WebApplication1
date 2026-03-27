using System.Text.Json;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public static class StoreContextSeeding
    {
        public async static Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categoriesData = File.ReadAllText("H:\\DEPI\\Web Api\\WebApplication1\\WebApplication1\\Data\\Seeding\\Categories.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories is null)
                    return;

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
        }
    }
}
