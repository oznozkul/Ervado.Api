using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovementById
{
    public class GetStockMovementByIdQueryHandler : IRequestHandler<GetStockMovementByIdQuery, Response<StockMovementDetailDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetStockMovementByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<StockMovementDetailDto>> Handle(GetStockMovementByIdQuery request, CancellationToken cancellationToken)
        {
            var stockMovement = await _dbContext.StockMovements
                .Include(sm => sm.Inventory)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(sm => sm.Id == request.Id && !sm.IsDeleted, cancellationToken);

            if (stockMovement == null)
            {
                return Response<StockMovementDetailDto>.Failure("Stock movement not found.");
            }

            var result = new StockMovementDetailDto
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
                StockLevelAfterMovement = CalculateStockLevelAfterMovement(stockMovement),
                CreatedDate = stockMovement.CreatedDate,
                CreatedByUserName = GetUserNameById(stockMovement.CreatedUserId)
            };

            return Response<StockMovementDetailDto>.Success(result);
        }

        private int CalculateStockLevelAfterMovement(Domain.Entities.StockMovement movement)
        {
            // This should be improved to find the actual stock level after the movement by using movement history
            // For now, using a simple estimation based on current stock and the movement
            if (movement.Inventory == null)
                return 0;

            switch (movement.Type)
            {
                case Domain.Entities.MovementType.Purchase:
                case Domain.Entities.MovementType.Return:
                case Domain.Entities.MovementType.Initial:
                    return movement.Inventory.Quantity - movement.Quantity; // Current stock minus what was added
                case Domain.Entities.MovementType.Sale:
                case Domain.Entities.MovementType.Transfer:
                case Domain.Entities.MovementType.Waste:
                    return movement.Inventory.Quantity + movement.Quantity; // Current stock plus what was removed
                case Domain.Entities.MovementType.Adjustment:
                    return movement.Quantity; // The quantity was set directly
                default:
                    return movement.Inventory.Quantity;
            }
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