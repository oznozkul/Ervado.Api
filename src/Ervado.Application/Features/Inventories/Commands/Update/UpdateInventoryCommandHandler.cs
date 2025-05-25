using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Commands.Update
{
    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateInventoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _dbContext.Inventories
                .FirstOrDefaultAsync(i => i.Id == request.Id && !i.IsDeleted, cancellationToken);

            if (inventory == null)
            {
                return Response.Failure("Inventory entry not found.");
            }

            // Update inventory details (excluding quantity which is handled by stock movements)
            inventory.MinimumStockLevel = request.MinimumStockLevel;
            inventory.MaximumStockLevel = request.MaximumStockLevel;
            inventory.Location = request.Location;
            inventory.Warehouse = request.Warehouse;
            inventory.Shelf = request.Shelf;
            inventory.Notes = request.Notes;
            inventory.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Inventory updated successfully.");
        }
    }
} 