using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.StockMovements.Commands.Create
{
    public class CreateStockMovementCommandHandler : IRequestHandler<CreateStockMovementCommand, Response<CreateStockMovementResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateStockMovementCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateStockMovementResponse>> Handle(CreateStockMovementCommand request, CancellationToken cancellationToken)
        {
            // Validate if inventory exists
            var inventory = await _dbContext.Inventories
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i => i.Id == request.InventoryId && !i.IsDeleted, cancellationToken);

            if (inventory == null)
            {
                return Response<CreateStockMovementResponse>.Failure("The specified inventory does not exist.");
            }

            // Validate quantity based on movement type
            if (request.Type == MovementType.Sale || request.Type == MovementType.Transfer || request.Type == MovementType.Waste)
            {
                if (request.Quantity > inventory.Quantity)
                {
                    return Response<CreateStockMovementResponse>.Failure("Not enough stock available for this operation.");
                }
            }

            // Create stock movement
            var stockMovement = new StockMovement
            {
                InventoryId = request.InventoryId,
                Quantity = request.Quantity,
                Type = request.Type,
                Reference = request.Reference,
                Notes = request.Notes,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.StockMovements.Add(stockMovement);

            // Update inventory quantity
            switch (request.Type)
            {
                case MovementType.Purchase:
                case MovementType.Return:
                case MovementType.Initial:
                    inventory.Quantity += request.Quantity;
                    break;
                case MovementType.Sale:
                case MovementType.Transfer:
                case MovementType.Waste:
                    inventory.Quantity -= request.Quantity;
                    break;
                case MovementType.Adjustment:
                    // For adjustment, the quantity is the new absolute value
                    inventory.Quantity = request.Quantity;
                    break;
            }

            inventory.LastStockUpdateDate = DateTime.UtcNow;
            inventory.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateStockMovementResponse>.Success(new CreateStockMovementResponse
            {
                Id = stockMovement.Id,
                InventoryId = stockMovement.InventoryId,
                ProductName = inventory.Product?.Name ?? string.Empty,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                NewStockLevel = inventory.Quantity,
                CreatedDate = stockMovement.CreatedDate
            });
        }
    }
} 