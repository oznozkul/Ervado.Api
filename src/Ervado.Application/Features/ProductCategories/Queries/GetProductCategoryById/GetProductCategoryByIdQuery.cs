using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public record GetProductCategoryByIdQuery : IRequest<Response<ProductCategoryDto>>
    {
        public int Id { get; init; }
    }
} 