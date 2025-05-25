using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Brands.Commands.Update
{
    public record UpdateBrandCommand : IRequest<Response>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string LogoUrl { get; init; } = string.Empty;
        public bool IsActive { get; init; }
    }
} 