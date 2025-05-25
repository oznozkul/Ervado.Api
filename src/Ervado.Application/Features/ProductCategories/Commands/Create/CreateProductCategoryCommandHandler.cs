using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Commands.Create
{
    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, Response<CreateProductCategoryResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateProductCategoryCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateProductCategoryResponse>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            // Validate parent category if provided
            if (request.ParentCategoryId.HasValue)
            {
                var parentExists = await _dbContext.ProductCategories.AnyAsync(c => c.Id == request.ParentCategoryId.Value, cancellationToken);
                if (!parentExists)
                {
                    return Response<CreateProductCategoryResponse>.Failure("The specified parent category does not exist.");
                }
            }

            var category = new ProductCategory
            {
                Name = request.Name,
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.ProductCategories.Add(category);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateProductCategoryResponse>.Success(new CreateProductCategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            });
        }
    }
} 