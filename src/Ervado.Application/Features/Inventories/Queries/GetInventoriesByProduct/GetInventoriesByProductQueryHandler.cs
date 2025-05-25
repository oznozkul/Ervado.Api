using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoriesByProduct
{
    public class GetInventoriesByProductQueryHandler : IRequestHandler<GetInventoriesByProductQuery, Response<List<ProductInventoryDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetInventoriesByProductQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<ProductInventoryDto>>> Handle(GetInventoriesByProductQuery request, CancellationToken cancellationToken)
        {
            // Validate if product exists
            var productExists = await _dbContext.Products
                .AnyAsync(p => p.Id == request.ProductId && !p.IsDeleted, cancellationToken);

            if (!productExists)
            {
                return Response<List<ProductInventoryDto>>.Failure("The specified product does not exist.");
            }

            var inventories = await _dbContext.Inventories
                .Where(i => i.ProductId == request.ProductId && !i.IsDeleted)
                .Select(i => new ProductInventoryDto
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    MinimumStockLevel = i.MinimumStockLevel,
                    MaximumStockLevel = i.MaximumStockLevel,
                    Location = i.Location,
                    Warehouse = i.Warehouse,
                    Shelf = i.Shelf,
                    LastStockUpdateDate = i.LastStockUpdateDate
                })
                .ToListAsync(cancellationToken);

            return Response<List<ProductInventoryDto>>.Success(inventories);
        }
    }
} 