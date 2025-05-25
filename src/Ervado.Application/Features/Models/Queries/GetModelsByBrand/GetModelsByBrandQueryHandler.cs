using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Queries.GetModelsByBrand
{
    public class GetModelsByBrandQueryHandler : IRequestHandler<GetModelsByBrandQuery, Response<List<ModelListItemDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetModelsByBrandQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<ModelListItemDto>>> Handle(GetModelsByBrandQuery request, CancellationToken cancellationToken)
        {
            // Validate if brand exists
            var brandExists = await _dbContext.Brands
                .AnyAsync(b => b.Id == request.BrandId && !b.IsDeleted, cancellationToken);

            if (!brandExists)
            {
                return Response<List<ModelListItemDto>>.Failure("The specified brand does not exist.");
            }

            var query = _dbContext.Models
                .Where(m => m.BrandId == request.BrandId && !m.IsDeleted);

            // Filter by active status
            if (!request.IncludeInactive)
            {
                query = query.Where(m => m.IsActive);
            }

            var models = await query
                .OrderBy(m => m.Name)
                .Select(m => new ModelListItemDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    IsActive = m.IsActive,
                    ProductCount = _dbContext.Products.Count(p => p.ModelId == m.Id && !p.IsDeleted)
                })
                .ToListAsync(cancellationToken);

            return Response<List<ModelListItemDto>>.Success(models);
        }
    }
} 