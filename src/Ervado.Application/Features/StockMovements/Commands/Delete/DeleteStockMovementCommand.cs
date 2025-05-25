using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.StockMovements.Commands.Delete
{
    public record DeleteStockMovementCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 