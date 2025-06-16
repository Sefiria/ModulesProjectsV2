using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class MongoService
    {

        private readonly IMongoCollection<Chunk> _collection;

        public MongoService()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("WorldMap");
            _collection = database.GetCollection<Chunk>("Chunks");
        }

        public void SaveChunk(Chunk chunk)
        {
            var filter = Builders<Chunk>.Filter.Eq(c => c.ChunkX, chunk.ChunkX) & Builders<Chunk>.Filter.Eq(c => c.ChunkY, chunk.ChunkY);
            _collection.ReplaceOne(filter, chunk, new ReplaceOptions { IsUpsert = true });
        }

        public Chunk LoadChunk(int chunkX, int chunkY)
        {
            var filter = Builders<Chunk>.Filter.Eq(c => c.ChunkX, chunkX) & Builders<Chunk>.Filter.Eq(c => c.ChunkY, chunkY);
            var chunk = _collection.Find(filter).FirstOrDefault();

            if (chunk == null) return null;

            return chunk;
        }
    }
}
