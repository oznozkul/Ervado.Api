using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Response<List<ProductListDto>>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<List<ProductListDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Include(p => p.Model)
                .Where(p => !p.IsDeleted);

            // Apply filters
            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategoryId == request.CategoryId.Value);
            }

            if (request.BrandId.HasValue)
            {
                query = query.Where(p => p.BrandId == request.BrandId.Value);
            }

            if (!request.IncludeInactive)
            {
                query = query.Where(p => p.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.SKU.ToLower().Contains(searchTerm) ||
                    p.Barcode.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm));
            }

            // Apply pagination
            var totalItems = await query.CountAsync(cancellationToken);
            var totalPages = (totalItems + request.PageSize - 1) / request.PageSize;

            var products = await query
                .OrderBy(p => p.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Barcode = p.Barcode,
                    PurchasePrice = p.PurchasePrice,
                    SalePrice = p.SalePrice,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    UnitType = p.UnitType,
                    CategoryName = p.ProductCategory != null ? p.ProductCategory.Name : string.Empty,
                    BrandName = p.Brand != null ? p.Brand.Name : string.Empty,
                    ModelName = p.Model != null ? p.Model.Name : string.Empty
                })
                .ToListAsync(cancellationToken);

            // TODO: Add pagination metadata to the response

            return Response<List<ProductListDto>>.Success(products);
        }
    }
} 