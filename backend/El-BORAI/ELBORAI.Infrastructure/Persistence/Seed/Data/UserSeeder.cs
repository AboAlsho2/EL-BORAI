using System.Text.Json;
using ELBORAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.Infrastructure.Persistence.Seed;

public static class UserSeeder
{
    public static async Task SeedAsync(ElBoraiDbContext context)
    {
        const string merchantEmail = "merchant@elborai.com";

        var existingUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == merchantEmail);

        if (existingUser != null)
            return;

        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence",
            "Seed",
            "Data",
            "users.json");

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var users = JsonSerializer.Deserialize<List<User>>(json, options);
        if (users == null || users.Count == 0)
            return;

        await context.Users.AddRangeAsync(users);

        await context.SaveChangesAsync();
    }
}