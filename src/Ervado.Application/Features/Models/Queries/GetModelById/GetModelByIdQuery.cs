using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Models.Queries.GetModelById
{
    public record GetModelByIdQuery : IRequest<Response<ModelDto>>
    {
        public int Id { get; init; }
    }
} 