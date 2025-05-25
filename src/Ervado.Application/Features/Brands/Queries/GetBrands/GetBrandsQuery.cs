using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.Brands.Queries.GetBrands
{
    public record GetBrandsQuery : IRequest<Response<List<BrandListDto>>>
    {
        public bool IncludeInactive { get; init; } = false;
        public string SearchTerm { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
} 