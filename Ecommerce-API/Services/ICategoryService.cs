using Ecommerce_API.Requests;

namespace Ecommerce_API.Services
{
    public interface ICategoryService
    {
        Task<IResult> GetAllCategories();
        Task<IResult> GetCategoryById(int id);
        Task<IResult> UpdateCategory(int id, CategoryRequest request);
        Task<IResult> CreateCategory(CategoryRequest request);
        Task<IResult> GetCategoriesWithProductNumbers();
        Task<IResult> DeleteCategory(int id);
    }
}
