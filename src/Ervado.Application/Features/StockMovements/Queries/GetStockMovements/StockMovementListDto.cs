using Ervado.Domain.Entities;
using System;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovements
{
    public class StockMovementListDto
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        
        // Product information
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        
        // Movement details
        public int Quantity { get; set; }
        public MovementType Type { get; set; }
        public string TypeName => Type.ToString();
        public string Reference { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
        // User who created the movement
        public string CreatedByUserName { get; set; } = string.Empty;
    }
} 