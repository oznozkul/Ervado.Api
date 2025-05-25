using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Models.Commands.Create
{
    public record CreateModelCommand : IRequest<Response<CreateModelResponse>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int BrandId { get; init; }
        public bool IsActive { get; init; } = true;
    }
} 