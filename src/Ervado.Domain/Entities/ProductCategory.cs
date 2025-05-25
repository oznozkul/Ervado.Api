using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductCategory ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<ProductCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
