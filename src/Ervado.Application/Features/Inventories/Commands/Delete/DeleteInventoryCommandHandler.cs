using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Inventories.Commands.Delete
{
    public class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteInventoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _dbContext.Inventories
                .FirstOrDefaultAsync(i => i.Id == request.Id && !i.IsDeleted, cancellationToken);

            if (inventory == null)
            {
                return Response.Failure("Inventory entry not found.");
            }

            // Check if there are any stock movements associated with this inventory
            var hasMovements = await _dbContext.StockMovements
                .AnyAsync(sm => sm.InventoryId == request.Id && !sm.IsDeleted, cancellationToken);

            if (hasMovements)
            {
                return Response.Failure("Cannot delete inventory because it has associated stock movements.");
            }

            // Soft delete
            inventory.IsDeleted = true;
            inventory.DeleteDate = (int)DateTime.UtcNow.Ticks;
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Inventory deleted successfully.");
        }
    }
} 