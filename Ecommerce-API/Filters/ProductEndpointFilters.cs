using Ecommerce_API.Data;
using Ecommerce_API.Requests;
using Ecommerce_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Filters
{
    public class ProductEndpointFilters
    {
        public async ValueTask<object?> ValidateUpdateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<ProductRequest>(2);
            return await ValidateRequest(request, context, next);
        }
        public async ValueTask<object?> ValidateCreateRequest(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var request = context.GetArgument<ProductRequest>(1);
            return await ValidateRequest(request, context, next);
        }
        private async ValueTask<object?> ValidateRequest(ProductRequest request, EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (request.Price < 0 || string.IsNullOrEmpty(request.Name)
                || string.IsNullOrEmpty(request.Description))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    {"Price", new [] { "Price should be greater than 0"} },
                    {"Name", new [] {"Name should not be empty"} },
                    {"Description", new [] {"Description Should Not be empty" } }
                });
            }

            return await next(context);
        }
    }
}

