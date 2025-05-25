using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Brands.Commands.Create
{
    public record CreateBrandCommand : IRequest<Response<CreateBrandResponse>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string LogoUrl { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
} 