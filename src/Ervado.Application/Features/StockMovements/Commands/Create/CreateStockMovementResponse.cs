using Ervado.Domain.Entities;
using System;

namespace Ervado.Application.Features.StockMovements.Commands.Create
{
    public record CreateStockMovementResponse
    {
        public int Id { get; init; }
        public int InventoryId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public MovementType Type { get; init; }
        public int NewStockLevel { get; init; }
        public DateTime CreatedDate { get; init; }
    }
} 