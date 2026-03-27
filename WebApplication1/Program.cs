using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Helper;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. IOC (Inversion of control)

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register DbContext as a service in IoC inversion of Control
            builder.Services.AddDbContext<ApplicationDbContext>(options =>

                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"))
                );

            //builder.Services.AddAutoMapper(typeof(ProductProfile));
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            });

            var app = builder.Build();

            // Automatic Migration
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                using var dbContext = services.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();
                await StoreContextSeeding.SeedAsync(dbContext); // Data Seeding
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, ex.Message);
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
