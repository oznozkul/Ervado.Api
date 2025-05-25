using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovements
{
    public record GetStockMovementsQuery : IRequest<Response<List<StockMovementListDto>>>
    {
        public int? InventoryId { get; init; }
        public int? ProductId { get; init; }
        public MovementType? Type { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }
        public string SearchTerm { get; init; } = string.Empty;
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
} 