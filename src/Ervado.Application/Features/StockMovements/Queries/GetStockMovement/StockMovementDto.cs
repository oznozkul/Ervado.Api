using Ervado.Domain.Entities;
using System;

namespace Ervado.Application.Features.StockMovements.Queries.GetStockMovement
{
    public class StockMovementDto
    {
        public int Id { get; set; }
        
        // Inventory and product information
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        
        // Movement details
        public int Quantity { get; set; }
        public MovementType Type { get; set; }
        public string TypeName => Type.ToString();
        public string Reference { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        
        // Location information
        public string Location { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        
        // Dates and user information
        public DateTime CreatedDate { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        
        // Stock levels
        public int StockLevelBefore { get; set; }
        public int StockLevelAfter { get; set; }
    }
} 