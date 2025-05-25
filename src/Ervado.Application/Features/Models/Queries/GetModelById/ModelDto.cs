using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Queries.GetModelById
{
    public class ModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Brand information
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string BrandLogoUrl { get; set; } = string.Empty;
        
        // Additional info
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        
        // Related counts
        public int ProductCount { get; set; }
    }
} 