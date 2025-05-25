using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class BaseEntity
    {
        public int Id {  get; set; }
        public int FirmId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeleteUserId { get; set; }
        public int? DeleteDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
