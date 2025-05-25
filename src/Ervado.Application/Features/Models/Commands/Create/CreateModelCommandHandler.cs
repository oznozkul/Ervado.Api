using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Commands.Create
{
    public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand, Response<CreateModelResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateModelCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateModelResponse>> Handle(CreateModelCommand request, CancellationToken cancellationToken)
        {
            // Validate if brand exists
            var brand = await _dbContext.Brands
                .FirstOrDefaultAsync(b => b.Id == request.BrandId, cancellationToken);

            if (brand == null)
            {
                return Response<CreateModelResponse>.Failure("The specified brand does not exist.");
            }

            // Check if model name already exists for this brand
            var modelExists = await _dbContext.Models
                .AnyAsync(m => m.BrandId == request.BrandId && m.Name == request.Name, cancellationToken);

            if (modelExists)
            {
                return Response<CreateModelResponse>.Failure($"A model with the name '{request.Name}' already exists for this brand.");
            }

            var model = new Model
            {
                Name = request.Name,
                Description = request.Description,
                BrandId = request.BrandId,
                IsActive = request.IsActive,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.Models.Add(model);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateModelResponse>.Success(new CreateModelResponse
            {
                Id = model.Id,
                Name = model.Name,
                BrandName = brand.Name
            });
        }
    }
} 