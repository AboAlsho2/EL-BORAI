
using ELBORAI.API.Exceptions;
using ELBORAI.Application.Interfaces;
using ELBORAI.Application.Interfaces.Repositories;
using ELBORAI.Application.Interfaces.Services;
using ELBORAI.Application.Services;
using ELBORAI.Application.Validators.Products;
using ELBORAI.Infrastructure.Persistence;
using ELBORAI.Infrastructure.Persistence.Repositories;
using ELBORAI.Infrastructure.Persistence.Seed;
using ELBORAI.Infrastructure.Services;
using FluentValidation;
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

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Add DbContext
            builder.Services.AddDbContext<ElBoraiDbContext>
                (options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpContextAccessor();


            //Dependency Injection 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            builder.Services.AddScoped(typeof(ValidationFilter<>));

            var app = builder.Build();

            app.UseExceptionHandler();

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
