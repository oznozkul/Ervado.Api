using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Brands.Queries.GetBrandById
{
    public record GetBrandByIdQuery : IRequest<Response<BrandDto>>
    {
        public int Id { get; init; }
    }
} 