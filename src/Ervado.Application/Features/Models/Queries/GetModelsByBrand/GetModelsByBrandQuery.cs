using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.Models.Queries.GetModelsByBrand
{
    public record GetModelsByBrandQuery : IRequest<Response<List<ModelListItemDto>>>
    {
        public int BrandId { get; init; }
        public bool IncludeInactive { get; init; } = false;
    }
} 