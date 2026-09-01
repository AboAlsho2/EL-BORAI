
using ELBORAI.Infrastructure.Persistence;
using ELBORAI.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace ELBORAI.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add DbContext
            builder.Services.AddDbContext<ElBoraiDbContext>
                (options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
            


            var app = builder.Build();


            // Apply EF Core migrations automatically
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ElBoraiDbContext>();

                dbContext.Database.Migrate();

                await DataSeeder.SeedAsync(dbContext);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
