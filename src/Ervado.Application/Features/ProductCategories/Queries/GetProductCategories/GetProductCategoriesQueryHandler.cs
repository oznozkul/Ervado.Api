using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategories
{
    public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, Response<List<ProductCategoryListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductCategoriesQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<ProductCategoryListDto>>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.ProductCategories
                .Where(c => !c.IsDeleted);

            // Filter by parent category if specified
            if (request.ParentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == request.ParentCategoryId);
            }
            else
            {
                // If no parent category specified, get root categories (those with no parent)
                query = query.Where(c => c.ParentCategoryId == null);
            }

            // Filter by active status
            if (!request.IncludeInactive)
            {
                query = query.Where(c => !c.IsDeleted);
            }

            // Search by name or description
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(c => 
                    c.Name.ToLower().Contains(searchTerm) || 
                    c.Description.ToLower().Contains(searchTerm));
            }

            // Get total count for pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            // Get categories with paging
            var categories = await query
                .OrderBy(c => c.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new ProductCategoryListDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    CreatedDate = c.CreatedDate,
                    IsActive = !c.IsDeleted,
                    ProductCount = _dbContext.Products.Count(p => p.ProductCategoryId == c.Id && !p.IsDeleted),
                    SubcategoriesCount = _dbContext.ProductCategories.Count(sc => sc.ParentCategoryId == c.Id && !sc.IsDeleted),
                    Level = c.ParentCategoryId.HasValue ? 1 : 0 // Simple level calculation
                })
                .ToListAsync(cancellationToken);

            // Get parent category names
            var parentIds = categories
                .Where(c => c.ParentCategoryId.HasValue)
                .Select(c => c.ParentCategoryId.Value)
                .Distinct()
                .ToList();

            if (parentIds.Any())
            {
                var parents = await _dbContext.ProductCategories
                    .Where(c => parentIds.Contains(c.Id))
                    .Select(c => new { c.Id, c.Name })
                    .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken);

                foreach (var category in categories.Where(c => c.ParentCategoryId.HasValue))
                {
                    if (parents.TryGetValue(category.ParentCategoryId.Value, out var parentName))
                    {
                        category.ParentCategoryName = parentName;
                    }
                }
            }

            return Response<List<ProductCategoryListDto>>.Success(categories);
        }
    }
} 