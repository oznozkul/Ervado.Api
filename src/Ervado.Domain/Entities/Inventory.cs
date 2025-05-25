using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        public string Location { get; set; }
        public string Warehouse { get; set; }
        public string Shelf { get; set; }
        public DateTime LastStockUpdateDate { get; set; }
        public string Notes { get; set; }
        
        // For tracking stock movements
        public ICollection<StockMovement> StockMovements { get; set; }
    }
} 