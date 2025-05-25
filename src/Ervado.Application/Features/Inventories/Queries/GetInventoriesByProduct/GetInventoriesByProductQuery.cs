using Ervado.Application.Common.Models;
using MediatR;
using System.Collections.Generic;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoriesByProduct
{
    public record GetInventoriesByProductQuery : IRequest<Response<List<ProductInventoryDto>>>
    {
        public int ProductId { get; init; }
    }
} 