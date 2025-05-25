using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Commands.Delete
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteBrandCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _dbContext.Brands
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (brand == null)
            {
                return Response.Failure("Brand not found.");
            }

            // Check if there are any products associated with this brand
            var hasProducts = await _dbContext.Products
                .AnyAsync(p => p.BrandId == request.Id, cancellationToken);

            if (hasProducts)
            {
                return Response.Failure("Cannot delete brand because it has associated products.");
            }

            // Soft delete
            brand.IsDeleted = true;
            brand.DeleteDate = (int)DateTime.UtcNow.Ticks;
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Brand deleted successfully.");
        }
    }
} 