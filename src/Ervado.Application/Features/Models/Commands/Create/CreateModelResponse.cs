using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Models.Commands.Create
{
    public record CreateModelResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string BrandName { get; init; } = string.Empty;
    }
} 