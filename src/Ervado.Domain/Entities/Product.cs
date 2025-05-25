using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal TaxRate { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        
        public UnitType UnitType { get; set; }
        public decimal UnitValue { get; set; } // For weight/volume specific values
        
        // Foreign keys
        public int ProductCategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        
        // Navigation properties
        public ProductCategory ProductCategory { get; set; }
        public Brand Brand { get; set; }
        public Model Model { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
    
    public enum UnitType
    {
        Piece = 1,    // Adet
        Gram = 2,     // Gram
        Kilogram = 3, // Kilogram
        Liter = 4,    // Litre
        Milliliter = 5, // Mililitre
        Meter = 6,    // Metre
        Centimeter = 7, // Santimetre
        Box = 8,      // Kutu
        Pair = 9,     // Çift
        Pack = 10     // Paket
    }
} 