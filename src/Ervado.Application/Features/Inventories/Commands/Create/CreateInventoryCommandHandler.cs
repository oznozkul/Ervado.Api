using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Commands.Create
{
    public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, Response<CreateInventoryResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateInventoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateInventoryResponse>> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
        {
            // Validate if product exists
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && !p.IsDeleted, cancellationToken);

            if (product == null)
            {
                return Response<CreateInventoryResponse>.Failure("The specified product does not exist.");
            }

            // Validate if inventory already exists for this product
            var inventoryExists = await _dbContext.Inventories
                .AnyAsync(i => i.ProductId == request.ProductId && !i.IsDeleted, cancellationToken);

            if (inventoryExists)
            {
                return Response<CreateInventoryResponse>.Failure("An inventory entry already exists for this product. Use update instead.");
            }

            // Create inventory entry
            var inventory = new Inventory
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                MinimumStockLevel = request.MinimumStockLevel,
                MaximumStockLevel = request.MaximumStockLevel,
                Location = request.Location,
                Warehouse = request.Warehouse,
                Shelf = request.Shelf,
                Notes = request.Notes,
                LastStockUpdateDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.Inventories.Add(inventory);

            // Create initial stock movement record
            if (request.Quantity > 0)
            {
                var stockMovement = new StockMovement
                {
                    InventoryId = inventory.Id,
                    Quantity = request.Quantity,
                    Type = MovementType.Initial,
                    Reference = "Initial inventory setup",
                    CreatedDate = DateTime.UtcNow
                    // CreatedUserId will be set from the user context
                };

                _dbContext.StockMovements.Add(stockMovement);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateInventoryResponse>.Success(new CreateInventoryResponse
            {
                Id = inventory.Id,
                ProductName = product.Name,
                Quantity = inventory.Quantity
            });
        }
    }
} 