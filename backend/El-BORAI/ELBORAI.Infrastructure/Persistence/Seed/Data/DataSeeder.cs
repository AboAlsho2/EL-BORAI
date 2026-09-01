namespace ELBORAI.Infrastructure.Persistence.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(ElBoraiDbContext context)
    {
        await CategorySeeder.SeedAsync(context);

        await UserSeeder.SeedAsync(context);

        await ProductSeeder.SeedAsync(context);
    }
}