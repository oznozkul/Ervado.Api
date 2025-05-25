namespace Ervado.Application.Features.Inventories.Commands.Create
{
    public record CreateInventoryResponse
    {
        public int Id { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
    }
} 