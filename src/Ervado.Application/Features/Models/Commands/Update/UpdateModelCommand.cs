using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Models.Commands.Update
{
    public record UpdateModelCommand : IRequest<Response>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int BrandId { get; init; }
        public bool IsActive { get; init; }
    }
} 