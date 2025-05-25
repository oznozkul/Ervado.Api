using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public DeleteProductCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Response.Failure("Product not found.");
            }

            // Soft delete
            product.IsDeleted = true;
            product.DeleteDate = (int)DateTime.UtcNow.Ticks; // Convert DateTime to int for DeleteDate
            // DeleteUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Product deleted successfully.");
        }
    }
} 