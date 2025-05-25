using System;

namespace Ervado.Application.Features.Inventories.Queries.GetInventoriesByProduct
{
    public class ProductInventoryDto
    {
        public int Id { get; set; }
        
        // Stock information
        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        
        // Location information
        public string Location { get; set; } = string.Empty;
        public string Warehouse { get; set; } = string.Empty;
        public string Shelf { get; set; } = string.Empty;
        
        // Status indicators
        public bool IsLowStock => Quantity < MinimumStockLevel;
        public bool IsOverStock => Quantity > MaximumStockLevel;
        public string StockStatus => 
            IsLowStock ? "Low" : 
            IsOverStock ? "Over" : "Normal";
        
        // Last update information
        public DateTime LastStockUpdateDate { get; set; }
    }
} 