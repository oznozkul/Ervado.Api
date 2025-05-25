using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class Firm : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentFirm { get; set; }
        public ApplicationUser User { get; set; }
    }
}
