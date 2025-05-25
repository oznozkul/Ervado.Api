using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.ProductCategories.Commands.Update
{
    public record UpdateProductCategoryCommand : IRequest<Response>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int? ParentCategoryId { get; init; }
    }
} 