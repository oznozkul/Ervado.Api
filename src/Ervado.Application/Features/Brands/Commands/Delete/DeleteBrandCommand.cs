using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Brands.Commands.Delete
{
    public record DeleteBrandCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 