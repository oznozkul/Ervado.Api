using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, Response<InventoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetInventoryByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<InventoryDto>> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _dbContext.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == request.Id && !i.IsDeleted, cancellationToken);

            if (inventory == null)
            {
                return Response<InventoryDto>.Failure("Inventory not found.");
            }

            var result = new InventoryDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                ProductName = inventory.Product?.Name ?? string.Empty,
                ProductSKU = inventory.Product?.SKU ?? string.Empty,
                Quantity = inventory.Quantity,
                MinimumStockLevel = inventory.MinimumStockLevel,
                MaximumStockLevel = inventory.MaximumStockLevel,
                Location = inventory.Location,
                Warehouse = inventory.Warehouse,
                Shelf = inventory.Shelf,
                Notes = inventory.Notes,
                LastStockUpdateDate = inventory.LastStockUpdateDate,
                CreatedDate = inventory.CreatedDate,
                UpdatedDate = inventory.UpdatedDate
            };

            // Get recent stock movements
            var recentMovements = await _dbContext.StockMovements
                .Where(sm => sm.InventoryId == inventory.Id && !sm.IsDeleted)
                .OrderByDescending(sm => sm.CreatedDate)
                .Take(10)
                .Select(sm => new RecentStockMovementDto
                {
                    Id = sm.Id,
                    Quantity = sm.Quantity,
                    Type = sm.Type.ToString(),
                    Reference = sm.Reference,
                    CreatedDate = sm.CreatedDate
                })
                .ToListAsync(cancellationToken);

            result.RecentMovements = recentMovements;

            return Response<InventoryDto>.Success(result);
        }
    }
} 