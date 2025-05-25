using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class StockMovement : BaseEntity
    {
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        
        public int Quantity { get; set; }
        
        public MovementType Type { get; set; }
        
        public string Reference { get; set; }
        public string Notes { get; set; }
    }
    
    public enum MovementType
    {
        Purchase = 1,
        Sale = 2,
        Return = 3,
        Adjustment = 4,
        Transfer = 5,
        Waste = 6,
        Initial = 7
    }
} 