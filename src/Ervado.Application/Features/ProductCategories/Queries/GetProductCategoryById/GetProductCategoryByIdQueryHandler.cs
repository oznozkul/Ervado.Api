using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, Response<ProductCategoryDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductCategoryByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<ProductCategoryDto>> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.ProductCategories
                .Where(c => c.Id == request.Id && !c.IsDeleted)
                .Select(c => new ProductCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    IsActive = !c.IsDeleted,
                    ProductCount = _dbContext.Products.Count(p => p.ProductCategoryId == c.Id && !p.IsDeleted),
                    SubcategoriesCount = _dbContext.ProductCategories.Count(sc => sc.ParentCategoryId == c.Id && !sc.IsDeleted)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
            {
                return Response<ProductCategoryDto>.Failure("Product category not found.");
            }

            // Get parent category name if exists
            if (category.ParentCategoryId.HasValue)
            {
                var parentCategory = await _dbContext.ProductCategories
                    .Where(c => c.Id == category.ParentCategoryId.Value)
                    .Select(c => new { c.Name })
                    .FirstOrDefaultAsync(cancellationToken);

                if (parentCategory != null)
                {
                    category.ParentCategoryName = parentCategory.Name;
                }
            }

            // Get subcategories if any
            var subcategories = await _dbContext.ProductCategories
                .Where(c => c.ParentCategoryId == request.Id && !c.IsDeleted)
                .Select(c => new SubcategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductCount = _dbContext.Products.Count(p => p.ProductCategoryId == c.Id && !p.IsDeleted)
                })
                .ToListAsync(cancellationToken);

            category.Subcategories = subcategories;

            return Response<ProductCategoryDto>.Success(category);
        }
    }
} 