using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery : IRequest<Response<ProductDto>>
    {
        public int Id { get; init; }
    }
} 