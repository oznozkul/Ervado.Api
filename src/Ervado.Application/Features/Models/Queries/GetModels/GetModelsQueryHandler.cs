using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Queries.GetModels
{
    public class GetModelsQueryHandler : IRequestHandler<GetModelsQuery, Response<List<ModelListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetModelsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<ModelListDto>>> Handle(GetModelsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Models
                .Where(m => !m.IsDeleted);

            // Filter by brand if specified
            if (request.BrandId.HasValue)
            {
                query = query.Where(m => m.BrandId == request.BrandId.Value);
            }

            // Filter by active status
            if (!request.IncludeInactive)
            {
                query = query.Where(m => m.IsActive);
            }

            // Search by name or description
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(m => 
                    m.Name.ToLower().Contains(searchTerm) || 
                    m.Description.ToLower().Contains(searchTerm));
            }

            // Get total count for pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            // Get models with paging
            var models = await query
                .OrderBy(m => m.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new ModelListDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    BrandId = m.BrandId,
                    CreatedDate = m.CreatedDate,
                    IsActive = m.IsActive,
                    ProductCount = _dbContext.Products.Count(p => p.ModelId == m.Id && !p.IsDeleted)
                })
                .ToListAsync(cancellationToken);

            // Get brand names
            var brandIds = models.Select(m => m.BrandId).Distinct().ToList();

            if (brandIds.Any())
            {
                var brands = await _dbContext.Brands
                    .Where(b => brandIds.Contains(b.Id))
                    .Select(b => new { b.Id, b.Name })
                    .ToDictionaryAsync(b => b.Id, b => b.Name, cancellationToken);

                foreach (var model in models)
                {
                    if (brands.TryGetValue(model.BrandId, out var brandName))
                    {
                        model.BrandName = brandName;
                    }
                }
            }

            return Response<List<ModelListDto>>.Success(models);
        }
    }
} 