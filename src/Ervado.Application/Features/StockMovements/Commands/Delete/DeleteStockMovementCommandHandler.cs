using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.StockMovements.Commands.Delete
{
    public class DeleteStockMovementCommandHandler : IRequestHandler<DeleteStockMovementCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteStockMovementCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteStockMovementCommand request, CancellationToken cancellationToken)
        {
            var stockMovement = await _dbContext.StockMovements
                .Include(sm => sm.Inventory)
                .FirstOrDefaultAsync(sm => sm.Id == request.Id && !sm.IsDeleted, cancellationToken);

            if (stockMovement == null)
            {
                return Response.Failure("Stock movement not found.");
            }

            // Revert the inventory quantity change
            if (stockMovement.Inventory != null)
            {
                switch (stockMovement.Type)
                {
                    case MovementType.Purchase:
                    case MovementType.Return:
                    case MovementType.Initial:
                        // These types add stock, so we need to subtract
                        stockMovement.Inventory.Quantity -= stockMovement.Quantity;
                        break;
                    case MovementType.Sale:
                    case MovementType.Transfer:
                    case MovementType.Waste:
                        // These types remove stock, so we need to add back
                        stockMovement.Inventory.Quantity += stockMovement.Quantity;
                        break;
                    case MovementType.Adjustment:
                        // For adjustment, we can't easily determine what to do
                        // We would need to find the previous stock level
                        return Response.Failure("Cannot delete adjustment stock movements. Please create a new adjustment instead.");
                }

                // Update inventory
                stockMovement.Inventory.LastStockUpdateDate = DateTime.UtcNow;
                stockMovement.Inventory.UpdatedDate = DateTime.UtcNow;
                // UpdatedUserId will be set from the user context
            }

            // Soft delete the stock movement
            stockMovement.IsDeleted = true;
            stockMovement.DeleteDate = (int)DateTime.UtcNow.Ticks;
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Stock movement deleted successfully.");
        }
    }
} 