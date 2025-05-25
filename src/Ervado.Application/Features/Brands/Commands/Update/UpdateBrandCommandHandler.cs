using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Commands.Update
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateBrandCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _dbContext.Brands
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (brand == null)
            {
                return Response.Failure("Brand not found.");
            }

            // Update brand properties
            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.LogoUrl = request.LogoUrl;
            brand.IsActive = request.IsActive;
            brand.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Brand updated successfully.");
        }
    }
} 