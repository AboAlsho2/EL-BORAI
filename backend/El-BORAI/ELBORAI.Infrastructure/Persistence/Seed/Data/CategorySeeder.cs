using System.Text.Json;
using ELBORAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.Infrastructure.Persistence.Seed;

public static class CategorySeeder
{
    public static async Task SeedAsync(ElBoraiDbContext context)
    {
        if (await context.Categories.AnyAsync())
            return;

        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence",
            "Seed",
            "Data",
            "categories.json");

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var categories = JsonSerializer.Deserialize<List<Category>>(json, options);
        if (categories == null || categories.Count == 0)
            return;

        await context.Categories.AddRangeAsync(categories);

        await context.SaveChangesAsync();
    }
}