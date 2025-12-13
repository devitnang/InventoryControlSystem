using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControlSystem.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fullName")]
        public string FullName { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        // Store hashed password
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        // Admin, Staff
        [BsonElement("role")]
        public string Role { get; set; }

        // Active or Disabled
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: Active / Disabled
        [BsonElement("status")]
        public string Status { get; set; } = "Active";
    }
}

