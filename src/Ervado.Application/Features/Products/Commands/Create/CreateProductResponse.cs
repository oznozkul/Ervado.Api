using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Products.Commands.Create
{
    public record CreateProductResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string SKU { get; init; } = string.Empty;
    }
} 