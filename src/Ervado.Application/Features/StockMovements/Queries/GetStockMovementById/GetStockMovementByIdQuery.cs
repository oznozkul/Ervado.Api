using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovementById
{
    public record GetStockMovementByIdQuery : IRequest<Response<StockMovementDetailDto>>
    {
        public int Id { get; init; }
    }
} 