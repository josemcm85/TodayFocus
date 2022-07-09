using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodayFocus.Model
{
    class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {

            //Tries to get db, if it is not available,
            //it creates the connection
            var client = new MongoClient();
            db = client.GetDatabase(database);

        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        public List<T> LoadRecords<T>(string table)
        {
            //Es como Select *
            var collection = db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }
    }
}
