using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.ProductCategories.Commands.Create
{
    public record CreateProductCategoryCommand : IRequest<Response<CreateProductCategoryResponse>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public int? ParentCategoryId { get; init; }
    }
} 