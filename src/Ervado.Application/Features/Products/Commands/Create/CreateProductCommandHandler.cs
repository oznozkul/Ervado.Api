using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<CreateProductResponse>>
    {
        private readonly IApplicationDbContext _dbContext;

        public CreateProductCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Validate if category exists
            var categoryExists = await _dbContext.ProductCategories.AnyAsync(c => c.Id == request.ProductCategoryId, cancellationToken);
            if (!categoryExists)
            {
                return Response<CreateProductResponse>.Failure("The specified product category does not exist.");
            }

            // Validate if brand exists (if provided)
            if (request.BrandId.HasValue)
            {
                var brandExists = await _dbContext.Brands.AnyAsync(b => b.Id == request.BrandId.Value, cancellationToken);
                if (!brandExists)
                {
                    return Response<CreateProductResponse>.Failure("The specified brand does not exist.");
                }
            }

            // Validate if model exists (if provided)
            if (request.ModelId.HasValue)
            {
                var modelExists = await _dbContext.Models.AnyAsync(m => m.Id == request.ModelId.Value, cancellationToken);
                if (!modelExists)
                {
                    return Response<CreateProductResponse>.Failure("The specified model does not exist.");
                }
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                SKU = request.SKU,
                Barcode = request.Barcode,
                PurchasePrice = request.PurchasePrice,
                SalePrice = request.SalePrice,
                TaxRate = request.TaxRate,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
                UnitType = request.UnitType,
                UnitValue = request.UnitValue,
                ProductCategoryId = request.ProductCategoryId,
                BrandId = request.BrandId,
                ModelId = request.ModelId,
                CreatedDate = DateTime.UtcNow
                // CreatedUserId will be set from the user context
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response<CreateProductResponse>.Success(new CreateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU
            });
        }
    }
} 