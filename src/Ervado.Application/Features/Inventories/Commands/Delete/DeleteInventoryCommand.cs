using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Inventories.Commands.Delete
{
    public record DeleteInventoryCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 