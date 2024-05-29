using Ecommerce_API.Data;
using Ecommerce_API.Models;
using Ecommerce_API.Requests;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly DataContext dbContext;

        public ProductService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IResult> CreateProduct(ProductRequest request)
        {
            if (!IsCategroyExists(request.CategoryId))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Category", new [] { "Category With this id does not exist"} },
                });
            }

            Product product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
            };
            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();

            return Results.Created($"/products/{product.Id}", product);
        }


        public async Task<IResult> DeleteProduct(int id)
        {
            Product? product = await dbContext.products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return Results.NotFound("Product Not Found");
            }
            dbContext.products.Remove(product);
            await dbContext.SaveChangesAsync();
            return Results.Ok("Product is deleted");
        }

        public async Task<IResult> GetAllProducts(decimal? minPrice, decimal? maxPrice, string? categoryName, string? productName, string? productDescription)
        {
            var query = dbContext.products
                    .Include(x => x.Category)
                    .AsQueryable();
            if (minPrice is not null)
            {
                query = query.Where(x => x.Price >= minPrice);
            }
            if (maxPrice is not null)
            {
                query = query.Where(x => x.Price <= maxPrice);
            }
            if (categoryName is not null)
            {
                query = query.Where(x => x.Category.Name.Contains(categoryName));
            }
            if (productName is not null)
            {
                query = query.Where(x => x.Name.Contains(productName));
            }
            if (productDescription is not null)
            {
                query = query.Where(x => x.Description.Contains(productDescription));
            }
            return Results.Ok(await query.ToListAsync());
        }

        public async Task<IResult> GetAverage()
        {
            var count = dbContext.products.Count();
            if (count == 0)
            {
                return Results.Ok(0);
            }
            return Results.Ok(await dbContext.products.AverageAsync(p => p.Price));
        }

        public async Task<IResult> GetProducById(int id)
        {
            var product = await dbContext
                .products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(product == null)
            {
                return Results.NotFound("Product Not Found");
            }
            return Results.Ok(product);
        }

        public async Task<IResult> UpdateProduct(int id, ProductRequest request)
        {
            var product = await dbContext.products.FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                return Results.NotFound("Product Not Found");
            }
            if (!IsCategroyExists(request.CategoryId))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Category", new [] { "Category With this id does not exist"} },
                });
            }
            product.Update(request.Name, request.Description, request.Price, request.CategoryId);
            await dbContext.SaveChangesAsync();
            return Results.Ok(product);
        }

        private bool IsCategroyExists(int CategoryId) { 
            return dbContext
                .categories
                .FirstOrDefaultAsync(c => c.Id == CategoryId) is null ? false : true;
        }
    }
}
