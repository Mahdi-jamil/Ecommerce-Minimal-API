using Ecommerce_API.Requests;

namespace Ecommerce_API.Services
{
    public interface IProductService
    {
        Task<IResult> GetAllProducts(decimal? minPrice, decimal? maxPrice,
    string? categoryName, string? productName, string? productDescription);
        Task<IResult> GetProducById(int id);
        Task<IResult> UpdateProduct(int id, ProductRequest request);
        Task<IResult> CreateProduct(ProductRequest request);
        Task<IResult> GetAverage();
        Task<IResult> DeleteProduct(int id);
    }
}
