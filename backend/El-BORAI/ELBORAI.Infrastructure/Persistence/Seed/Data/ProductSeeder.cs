using System.Text.Json;
using ELBORAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.Infrastructure.Persistence.Seed;

public static class ProductSeeder
{
    public static async Task SeedAsync(ElBoraiDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence",
            "Seed",
            "Data",
            "products.json");

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var productData = JsonSerializer.Deserialize<List<ProductSeedModel>>(
            json,
            options);

        if (productData == null || productData.Count == 0)
            return;

        var merchant = await context.Users
            .FirstOrDefaultAsync(u => u.Email == "merchant@elborai.com");

        if (merchant == null)
            throw new InvalidOperationException(
                "Seed merchant was not found.");

        var categories = await context.Categories
            .ToListAsync();

        var products = new List<Product>();

        foreach (var item in productData)
        {
            var category = categories.FirstOrDefault(
                c => c.Name == item.CategoryName);

            if (category == null)
            {
                throw new InvalidOperationException(
                    $"Category '{item.CategoryName}' was not found.");
            }

            products.Add(new Product
            {
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Stock = item.Stock,
                CategoryId = category.Id,
                MerchantId = merchant.Id
            });
        }

        await context.Products.AddRangeAsync(products);

        await context.SaveChangesAsync();
    }

    private class ProductSeedModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}