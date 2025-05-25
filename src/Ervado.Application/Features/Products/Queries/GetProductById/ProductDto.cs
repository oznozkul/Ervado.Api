using Ervado.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Queries.GetProductById
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TaxRate { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        public UnitType UnitType { get; set; }
        public decimal UnitValue { get; set; }
        
        // Related entities data
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; } = string.Empty;
        
        public int? BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        
        public int? ModelId { get; set; }
        public string ModelName { get; set; } = string.Empty;
    }
} 