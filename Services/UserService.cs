using MongoDB.Bson;
using MongoDB.Driver;
using cinemaReservation.Models;
using cinemaReservation.Helpers;

namespace cinemaReservation.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDB"));
            var mongoDatabase = mongoClient.GetDatabase("CinemaDatabase");

            _users = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task CreateAsync(User user)
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _users.InsertOneAsync(user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.Find(x => x.Email == email).FirstOrDefaultAsync();
        }

        // Bu yeni metot ile kullanıcıyı ID'sine göre bulabileceğiz
        public async Task<User> GetByIdAsync(string id)
        {
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
