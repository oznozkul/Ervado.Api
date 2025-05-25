using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Inventories.Commands.Update
{
    public record UpdateInventoryCommand : IRequest<Response>
    {
        public int Id { get; init; }
        public int MinimumStockLevel { get; init; }
        public int MaximumStockLevel { get; init; }
        public string Location { get; init; } = string.Empty;
        public string Warehouse { get; init; } = string.Empty;
        public string Shelf { get; init; } = string.Empty;
        public string Notes { get; init; } = string.Empty;
    }
} 