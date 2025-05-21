using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public int CompanyId { get; set; }
        public int? CreatedUserId { get; set; } 
        public DateTime CreatedDate   { get; set; } = DateTime.Now;
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate   { get; set; } 
        public int? DeletedUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
