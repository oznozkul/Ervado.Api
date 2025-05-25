using Ervado.Application.Common.Models;
using Ervado.Domain.Entities;
using MediatR;

namespace Ervado.Application.Features.Products.Commands.Update
{
    public record UpdateProductCommand : IRequest<Response>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
        public string Barcode { get; init; } = string.Empty;
        public decimal PurchasePrice { get; init; }
        public decimal SalePrice { get; init; }
        public decimal TaxRate { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        
        public UnitType UnitType { get; init; }
        public decimal UnitValue { get; init; }
        
        public int ProductCategoryId { get; init; }
        public int? BrandId { get; init; }
        public int? ModelId { get; init; }
    }
} 