using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Response<BrandDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetBrandByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<BrandDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _dbContext.Brands
                .Where(b => b.Id == request.Id && !b.IsDeleted)
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    LogoUrl = b.LogoUrl,
                    IsActive = b.IsActive,
                    CreatedDate = b.CreatedDate,
                    UpdatedDate = b.UpdatedDate,
                    ProductCount = _dbContext.Products.Count(p => p.BrandId == b.Id && !p.IsDeleted)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (brand == null)
            {
                return Response<BrandDto>.Failure("Brand not found.");
            }

            return Response<BrandDto>.Success(brand);
        }
    }
} 