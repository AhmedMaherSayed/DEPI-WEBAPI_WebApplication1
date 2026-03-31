using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Interceptors;
using WebApplication1.Helper;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            // Register DbContext as a service in IoC inversion of Control
            services.AddDbContext<ApplicationDbContext>(options =>

                    options.UseSqlServer(configuration.GetConnectionString("DefaultSqlConnection"))
                    .AddInterceptors(new SoftDeleteInterceptor(), new CreatedAtInterceptor())
                );

            //services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
