using Ecommerce_API.Data;
using Ecommerce_API.Models;
using Ecommerce_API.Requests;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext dbContext;

        public CategoryService(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IResult> CreateCategory(CategoryRequest request)
        {
            Category category = new Category
            {
                Name = request.Name
            };
            await dbContext.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/categories/{category.Id}", category);
        }

        public async Task<IResult> DeleteCategory(int id)
        {
            var category = await dbContext.categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
                return Results.NotFound("Category does not exist");
            
            dbContext.categories.Remove(category);
            await dbContext.SaveChangesAsync();
            return Results.Ok("Category and it's related products have been deleted");
        }

        public async Task<IResult> GetAllCategories()
        {
            return Results.Ok(await dbContext.categories.ToListAsync());
        }

        public async Task<IResult> GetCategoriesWithProductNumbers()
        {
            return Results.Ok(
                await dbContext.categories.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductNumber = dbContext.products.Count(p => p.CategoryId == c.Id)
                }
                ).ToListAsync());
        }
        
        public async Task<IResult> GetCategoryById(int id)
        {
            var category = await dbContext.categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return Results.NotFound("Category not found");
            }
            return Results.Ok(category);
        }

        public async Task<IResult> UpdateCategory(int id, CategoryRequest request)
        {
            var category = await dbContext.categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null)
            {
                return Results.NotFound("Category not found");
            }
            category.Update(request.Name);
            await dbContext.SaveChangesAsync();
            return Results.Ok(category);
        }
    }
}
