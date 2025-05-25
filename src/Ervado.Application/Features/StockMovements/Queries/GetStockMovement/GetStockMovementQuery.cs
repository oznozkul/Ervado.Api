using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovement
{
    public record GetStockMovementQuery : IRequest<Response<StockMovementDto>>
    {
        public int Id { get; init; }
    }
} 