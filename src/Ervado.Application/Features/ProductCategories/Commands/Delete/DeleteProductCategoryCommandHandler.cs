using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteProductCategoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Response.Failure("Product category not found.");
            }

            // Check if there are any products associated with this category
            var hasProducts = await _dbContext.Products
                .AnyAsync(p => p.ProductCategoryId == request.Id, cancellationToken);

            if (hasProducts)
            {
                return Response.Failure("Cannot delete category because it has associated products.");
            }

            // Check if there are any subcategories
            var hasSubcategories = await _dbContext.ProductCategories
                .AnyAsync(c => c.ParentCategoryId == request.Id, cancellationToken);

            if (hasSubcategories)
            {
                return Response.Failure("Cannot delete category because it has subcategories.");
            }

            // Soft delete
            category.IsDeleted = true;
            category.DeleteDate = (int)DateTime.UtcNow.Ticks;
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Product category deleted successfully.");
        }
    }
} 