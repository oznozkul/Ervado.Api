using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Commands.Update
{
    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateProductCategoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Response.Failure("Product category not found.");
            }

            // Validate parent category if provided
            if (request.ParentCategoryId.HasValue)
            {
                // Check if parent category exists
                var parentExists = await _dbContext.ProductCategories
                    .AnyAsync(c => c.Id == request.ParentCategoryId.Value, cancellationToken);
                
                if (!parentExists)
                {
                    return Response.Failure("The specified parent category does not exist.");
                }

                // Check for circular reference
                if (request.ParentCategoryId.Value == request.Id)
                {
                    return Response.Failure("A category cannot be its own parent.");
                }
            }

            // Update category properties
            category.Name = request.Name;
            category.Description = request.Description;
            category.ParentCategoryId = request.ParentCategoryId;
            category.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Product category updated successfully.");
        }
    }
} 