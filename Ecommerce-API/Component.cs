using Ecommerce_API.Data;
using Ecommerce_API.Services;
using Ecommerce_API.Services.Impl;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API
{
    public static class Component
    {
        public static IServiceCollection AddDependencies(
            this IServiceCollection services
            ,IConfiguration configuration)
        {

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PostgreSQLConnection")));
            
            services.AddScoped<IProductService, ProductService>()
                .AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
