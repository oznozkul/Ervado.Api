using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.ProductCategories.Commands.Create
{
    public record CreateProductCategoryResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
} 