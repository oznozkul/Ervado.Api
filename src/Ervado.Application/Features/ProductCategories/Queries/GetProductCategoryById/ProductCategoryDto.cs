using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategoryById
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Parent category info
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; } = string.Empty;
        
        // Additional info
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Related counts
        public int ProductCount { get; set; }
        public int SubcategoriesCount { get; set; }
        
        // Related collections (can be used for expanded view)
        public List<SubcategoryDto> Subcategories { get; set; } = new List<SubcategoryDto>();
    }
    
    // A simplified DTO for subcategories
    public class SubcategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
} 