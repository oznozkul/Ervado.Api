using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Commands.Delete
{
    public class DeleteModelCommandHandler : IRequestHandler<DeleteModelCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteModelCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _dbContext.Models
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (model == null)
            {
                return Response.Failure("Model not found.");
            }

            // Check if there are any products associated with this model
            var hasProducts = await _dbContext.Products
                .AnyAsync(p => p.ModelId == request.Id, cancellationToken);

            if (hasProducts)
            {
                return Response.Failure("Cannot delete model because it has associated products.");
            }

            // Soft delete
            model.IsDeleted = true;
            model.DeleteDate = (int)DateTime.UtcNow.Ticks;
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Model deleted successfully.");
        }
    }
} 