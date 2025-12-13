using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControlSystem.Models
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string CategoryId { get; set; } // store as string for easy binding
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; } = 0;
        public string ImageBase64 { get; set; } // optional
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
