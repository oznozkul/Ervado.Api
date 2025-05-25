using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategories
{
    public record GetProductCategoriesQuery : IRequest<Response<List<ProductCategoryListDto>>>
    {
        public int? ParentCategoryId { get; init; }
        public bool IncludeInactive { get; init; } = false;
        public string SearchTerm { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
} 