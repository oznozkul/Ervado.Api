using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery : IRequest<Response<List<ProductListDto>>>
    {
        public int? CategoryId { get; init; }
        public int? BrandId { get; init; }
        public bool IncludeInactive { get; init; } = false;
        public string SearchTerm { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
} 