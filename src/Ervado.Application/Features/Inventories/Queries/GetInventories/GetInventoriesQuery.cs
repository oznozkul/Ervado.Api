using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.Inventories.Queries.GetInventories
{
    public record GetInventoriesQuery : IRequest<Response<List<InventoryListDto>>>
    {
        public int? ProductId { get; init; }
        public int? ProductCategoryId { get; init; }
        public bool LowStockOnly { get; init; } = false;
        public bool IncludeInactive { get; init; } = false;
        public string SearchTerm { get; init; } = string.Empty;
        public string SortBy { get; init; } = "Name";
        public bool SortDescending { get; init; } = false;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
} 