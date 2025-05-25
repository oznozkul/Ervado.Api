using Ervado.Application.Common.Interfaces;
using Ervado.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateProductCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Response.Failure("Product not found.");
            }

            // Validate if category exists
            var categoryExists = await _dbContext.ProductCategories.AnyAsync(c => c.Id == request.ProductCategoryId, cancellationToken);
            if (!categoryExists)
            {
                return Response.Failure("The specified product category does not exist.");
            }

            // Validate if brand exists (if provided)
            if (request.BrandId.HasValue)
            {
                var brandExists = await _dbContext.Brands.AnyAsync(b => b.Id == request.BrandId.Value, cancellationToken);
                if (!brandExists)
                {
                    return Response.Failure("The specified brand does not exist.");
                }
            }

            // Validate if model exists (if provided)
            if (request.ModelId.HasValue)
            {
                var modelExists = await _dbContext.Models.AnyAsync(m => m.Id == request.ModelId.Value, cancellationToken);
                if (!modelExists)
                {
                    return Response.Failure("The specified model does not exist.");
                }
            }

            // Update product properties
            product.Name = request.Name;
            product.Description = request.Description;
            product.SKU = request.SKU;
            product.Barcode = request.Barcode;
            product.PurchasePrice = request.PurchasePrice;
            product.SalePrice = request.SalePrice;
            product.TaxRate = request.TaxRate;
            product.ImageUrl = request.ImageUrl;
            product.IsActive = request.IsActive;
            product.UnitType = request.UnitType;
            product.UnitValue = request.UnitValue;
            product.ProductCategoryId = request.ProductCategoryId;
            product.BrandId = request.BrandId;
            product.ModelId = request.ModelId;
            product.UpdatedDate = DateTime.UtcNow;
            // UpdatedUserId will be set from the user context

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Response.Success("Product updated successfully.");
        }
    }
} 