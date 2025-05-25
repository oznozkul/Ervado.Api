using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<ProductDto>>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetProductByIdQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Include(p => p.Model)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Response<ProductDto>.Failure("Product not found.");
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Barcode = product.Barcode,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                TaxRate = product.TaxRate,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                UnitType = product.UnitType,
                UnitValue = product.UnitValue,
                
                ProductCategoryId = product.ProductCategoryId,
                ProductCategoryName = product.ProductCategory?.Name ?? string.Empty,
                
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name ?? string.Empty,
                
                ModelId = product.ModelId,
                ModelName = product.Model?.Name ?? string.Empty
            };

            return Response<ProductDto>.Success(productDto);
        }
    }
} 