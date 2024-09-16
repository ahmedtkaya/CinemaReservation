using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cinemaReservation.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("uuid")]
        [BsonRepresentation(BsonType.String)]
        public Guid UUID { get; set; }


        public string Title { get; set; }
        public string Director { get; set; }
        public string ReleaseYear { get; set; }

        public Movie()
        {
            UUID = Guid.NewGuid();
        }
    }
}