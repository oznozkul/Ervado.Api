using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovement
{
    public class GetStockMovementQueryHandler : IRequestHandler<GetStockMovementQuery, Response<StockMovementDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetStockMovementQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<StockMovementDto>> Handle(GetStockMovementQuery request, CancellationToken cancellationToken)
        {
            var stockMovement = await _dbContext.StockMovements
                .Include(sm => sm.Inventory)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(sm => sm.Id == request.Id && !sm.IsDeleted, cancellationToken);

            if (stockMovement == null)
            {
                return Response<StockMovementDto>.Failure("Stock movement not found.");
            }

            var result = new StockMovementDto
            {
                Id = stockMovement.Id,
                InventoryId = stockMovement.InventoryId,
                ProductId = stockMovement.Inventory?.ProductId ?? 0,
                ProductName = stockMovement.Inventory?.Product?.Name ?? string.Empty,
                ProductSKU = stockMovement.Inventory?.Product?.SKU ?? string.Empty,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                Reference = stockMovement.Reference,
                Notes = stockMovement.Notes,
                Location = stockMovement.Inventory?.Location ?? string.Empty,
                Warehouse = stockMovement.Inventory?.Warehouse ?? string.Empty,
                CreatedDate = stockMovement.CreatedDate,
                CreatedByUserName = GetUserNameById(stockMovement.CreatedUserId),
                StockLevelBefore = CalculateStockLevelBefore(stockMovement),
                StockLevelAfter = CalculateStockLevelAfter(stockMovement)
            };

            return Response<StockMovementDto>.Success(result);
        }

        private int CalculateStockLevelBefore(Domain.Entities.StockMovement movement)
        {
            if (movement.Inventory == null)
                return 0;

            switch (movement.Type)
            {
                case Domain.Entities.MovementType.Purchase:
                case Domain.Entities.MovementType.Return:
                case Domain.Entities.MovementType.Initial:
                    return movement.Inventory.Quantity - movement.Quantity;
                case Domain.Entities.MovementType.Sale:
                case Domain.Entities.MovementType.Transfer:
                case Domain.Entities.MovementType.Waste:
                    return movement.Inventory.Quantity + movement.Quantity;
                case Domain.Entities.MovementType.Adjustment:
                    // For adjustment, we can't easily determine without history
                    return 0;
                default:
                    return movement.Inventory.Quantity;
            }
        }

        private int CalculateStockLevelAfter(Domain.Entities.StockMovement movement)
        {
            if (movement.Inventory == null)
                return 0;

            // If it's the latest movement, the after level is the current inventory level
            return movement.Inventory.Quantity;
        }

        private string GetUserNameById(int? userId)
        {
            if (!userId.HasValue)
                return string.Empty;

            // In a real application, we would look up the user's name from a user repository
            return $"User {userId}";
        }
    }
} 