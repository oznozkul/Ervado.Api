using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoryById
{
    public record GetInventoryByIdQuery : IRequest<Response<InventoryDto>>
    {
        public int Id { get; init; }
    }
} 