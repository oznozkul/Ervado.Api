using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Commands.Update
{
    public class UpdateModelCommandHandler : IRequestHandler<UpdateModelCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateModelCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
        {
            var model = await _dbContext.Models
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (model == null)
            {
                return Response.Failure("Model not found.");
            }

            // Validate if brand exists
            var brandExists = await _dbContext.Brands
                .AnyAsync(b => b.Id == request.BrandId, cancellationToken);

            if (!brandExists)
            {
                return Response.Failure("The specified brand does not exist.");
            }

            // Check if another model with the same name exists for this brand
            var duplicateExists = await _dbContext.Models
                .AnyAsync(m => m.Id != request.Id && m.BrandId == request.BrandId && m.Name == request.Name, cancellationToken);

            if (duplicateExists)
            {
                return Response.Failure($"Another model with the name '{request.Name}' already exists for this brand.");
            }

            // Update model properties
            model.Name = request.Name;
            model.Description = request.Description;
            model.BrandId = request.BrandId;
            model.IsActive = request.IsActive;
            model.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Model updated successfully.");
        }
    }
} 