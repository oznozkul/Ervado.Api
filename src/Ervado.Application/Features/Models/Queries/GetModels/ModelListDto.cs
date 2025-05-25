using System;

namespace Ervado.Application.Features.Models.Queries.GetModels
{
    public class ModelListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Brand info
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        
        // Additional info
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Related counts
        public int ProductCount { get; set; }
    }
} 