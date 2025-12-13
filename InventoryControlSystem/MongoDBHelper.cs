using InventoryControlSystem.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControlSystem
{
    public class MongoDBHelper
    {
        private static IMongoDatabase _database;
        private static MongoClient _client;

        // Change connection string and DB name here if needed
        private const string ConnectionString = "mongodb://127.0.0.1:27017";
        private const string DatabaseName = "inventorydb";

        static MongoDBHelper()
        {
            _client = new MongoClient(ConnectionString);
            _database = _client.GetDatabase(DatabaseName);
        }

        public static IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }

        public static IMongoCollection<User> GetUserCollection(string v)
        {
            return _database.GetCollection<User>(v);
        }
    }
}
