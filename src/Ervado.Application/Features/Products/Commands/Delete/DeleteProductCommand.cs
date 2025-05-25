using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Products.Commands.Delete
{
    public record DeleteProductCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 