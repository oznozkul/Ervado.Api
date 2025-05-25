using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.ProductCategories.Commands.Delete
{
    public record DeleteProductCategoryCommand : IRequest<Response>
    {
        public int Id { get; init; }
    }
} 