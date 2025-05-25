using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Queries.GetBrands
{
    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, Response<List<BrandListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetBrandsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<BrandListDto>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Brands
                .Where(b => !b.IsDeleted);

            // Apply filters
            if (!request.IncludeInactive)
            {
                query = query.Where(b => b.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(b => 
                    b.Name.ToLower().Contains(searchTerm) || 
                    b.Description.ToLower().Contains(searchTerm));
            }

            // Count total items for pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            // Apply pagination
            var brands = await query
                .OrderBy(b => b.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => new BrandListDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    LogoUrl = b.LogoUrl,
                    IsActive = b.IsActive,
                    CreatedDate = b.CreatedDate,
                    ProductCount = _dbContext.Products.Count(p => p.BrandId == b.Id && !p.IsDeleted)
                })
                .ToListAsync(cancellationToken);

            // You could add pagination metadata here if needed

            return Response<List<BrandListDto>>.Success(brands);
        }
    }
} 