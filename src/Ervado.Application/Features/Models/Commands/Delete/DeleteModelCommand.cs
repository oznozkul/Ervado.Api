using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Models.Commands.Delete
{
    public record DeleteModelCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 