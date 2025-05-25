using System;

namespace Ervado.Application.Features.ProductCategories.Queries.GetProductCategories
{
    public class ProductCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Parent category info (if any)
        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; } = string.Empty;
        
        // Additional info
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Counts
        public int ProductCount { get; set; }
        public int SubcategoriesCount { get; set; }
        
        // Hierarchy level (for UI display)
        public int Level { get; set; }
    }
} 