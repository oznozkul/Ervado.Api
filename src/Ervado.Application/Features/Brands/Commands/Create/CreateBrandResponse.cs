using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Brands.Commands.Create
{
    public record CreateBrandResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
} 