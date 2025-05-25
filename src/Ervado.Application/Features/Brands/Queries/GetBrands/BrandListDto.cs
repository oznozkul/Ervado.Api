using System;

namespace Ervado.Application.Features.Brands.Queries.GetBrands
{
    public class BrandListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Count of products associated with this brand
        public int ProductCount { get; set; }
    }
} 