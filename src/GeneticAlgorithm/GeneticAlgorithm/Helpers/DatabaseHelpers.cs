
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeneticAlgorithm.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GeneticAlgorithm.Helpers
{
    public class DatabaseHelpers
    {
        public static Evolution GetEvolution(string id)
        {
            var objectId = new ObjectId();

            if (ObjectId.TryParse(id, out objectId))
            {
                return GetAllSeeds().FirstOrDefault(p => p.Id == objectId);
            }

            return new Evolution();
        }

        public static List<Evolution> GetAllSeeds()
        {
            var list = GetSeedCollection().AsQueryable().ToList();
            return list;
        }

        public static IMongoCollection<Evolution> GetSeedCollection()
        {
            var col = GetDatabase().GetCollection<Models.Evolution>("Evolution");
            return col;
        }

        public static IMongoDatabase GetDatabase()
        {
            //Set MongoDb
            MongoClient client = new MongoClient();
            var db = client.GetDatabase("GeneticAlgorithm");
            return db;
        }
    }
}