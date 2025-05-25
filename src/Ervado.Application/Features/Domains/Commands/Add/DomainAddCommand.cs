using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Domains.Commands.Add
{
    public class DomainAddCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentFirm { get; set; }
    }
}
