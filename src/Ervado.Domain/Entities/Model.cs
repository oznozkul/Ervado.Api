using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class Model : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        
        // Foreign key for Brand
        public int BrandId { get; set; }
        
        // Navigation properties
        public Brand Brand { get; set; }
        public ICollection<Product> Products { get; set; }
    }
} 