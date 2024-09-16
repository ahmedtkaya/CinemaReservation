using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace cinemaReservation.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}