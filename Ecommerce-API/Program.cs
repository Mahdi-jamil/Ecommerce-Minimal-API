using Ecommerce_API;
using Ecommerce_API.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Ecommerce_API.Services;
using Ecommerce_API.Filters;
using Ecommerce_API.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

var productMapping = app.MapGroup("/products").WithTags("Products");
var categoryMapping = app.MapGroup("/categories").WithTags("Categories");

productMapping.MapPost("/", async (IProductService productService, ProductRequest productsRequest) =>
    await productService.CreateProduct(productsRequest))
    .AddEndpointFilter(new ProductEndpointFilters().ValidateCreateRequest)
    .WithOpenApi();


productMapping.MapPut("/{id}", async (IProductService productService, int id,
    ProductRequest request)
    => await productService.UpdateProduct(id, request))
    .AddEndpointFilter(new ProductEndpointFilters().ValidateUpdateRequest);


productMapping.MapGet("/{id}", async (IProductService productService, int id) =>
{
    var result = await productService.GetProducById(id);
    return result;
});
productMapping.MapGet("/average", async (IProductService productService)
    => await productService.GetAverage());
productMapping.MapDelete("/{id}", async (IProductService productService, int id)
    => await productService.DeleteProduct(id));



categoryMapping.MapGet("/", async (ICategoryService categoryService)
    => await categoryService.GetAllCategories());

categoryMapping.MapPut("/{id}", async (ICategoryService categoryService, int id, CategoryRequest request)
    => await categoryService.UpdateCategory(id, request))
    .AddEndpointFilter(new CategoryEndpointFilters().ValidateUpdateRequest);

categoryMapping.MapPost("/", async (ICategoryService categoryService, CategoryRequest categoryRequest) =>
    await categoryService.CreateCategory(categoryRequest))
    .AddEndpointFilter(new CategoryEndpointFilters().ValidateCreateRequest);

categoryMapping.MapGet("/{id}", async (ICategoryService categoryService, int id)
    => await categoryService.GetCategoryById(id));

categoryMapping.MapGet("/countOfProducts", async (ICategoryService categoryService)
    => await categoryService.GetCategoriesWithProductNumbers());

categoryMapping.MapDelete("/{id}", async (ICategoryService categoryService, int id)
    => await categoryService.DeleteCategory(id));

app.Run();

