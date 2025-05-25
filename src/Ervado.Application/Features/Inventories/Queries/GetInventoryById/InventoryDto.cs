using System;
using System.Collections.Generic;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoryById
{
    public class InventoryDto
    {
        public int Id { get; set; }
        
        // Product information
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSKU { get; set; } = string.Empty;
        
        // Inventory information
        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        public string Shelf { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime LastStockUpdateDate { get; set; }
        
        // Additional information
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        
        // Status indicators
        public bool IsLowStock => Quantity < MinimumStockLevel;
        public bool IsOverStock => Quantity > MaximumStockLevel;
        
        // Recent movements
        public List<RecentStockMovementDto> RecentMovements { get; set; } = new List<RecentStockMovementDto>();
    }
    
    public class RecentStockMovementDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
} 