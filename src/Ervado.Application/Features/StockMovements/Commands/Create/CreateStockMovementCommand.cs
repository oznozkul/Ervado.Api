using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;

namespace Ervado.Application.Features.StockMovements.Commands.Create
{
    public record CreateStockMovementCommand : IRequest<Response<CreateStockMovementResponse>>
    {
        public int InventoryId { get; init; }
        public int Quantity { get; init; }
        public MovementType Type { get; init; }
        public string Reference { get; init; } = string.Empty;
        public string Notes { get; init; } = string.Empty;
    }
} 