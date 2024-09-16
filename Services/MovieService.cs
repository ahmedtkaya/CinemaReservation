using MongoDB.Bson;
using MongoDB.Driver;
using cinemaReservation.Models;
using cinemaReservation.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace cinemaReservation.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MovieService(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDB"));
            var mongoDatabase = mongoClient.GetDatabase("CinemaDatabase");

            var indexKeysDefinition = Builders<Movie>.IndexKeys.Ascending(movie => movie.Title);
            var indexOptions = new CreateIndexOptions { Unique = true };
            var indexModel = new CreateIndexModel<Movie>(indexKeysDefinition, indexOptions);
            _movies = mongoDatabase.GetCollection<Movie>("Movies");
        }
        public async Task CreateAsync(Movie movie)
        {
            var existingMovie = await _movies.Find(x => x.Title == movie.Title).FirstOrDefaultAsync();
            if (existingMovie != null)
            {
                throw new Exception("A movie with this title already exists...");
            }

            await _movies.InsertOneAsync(movie);
        }

        public async Task<List<Movie>> GetAllMoviesAsync()
        {
            return await _movies.Find(movie => true).ToListAsync();
        }

        public async Task UpdateAsync(string id, Movie updatedMovie)
        {
            var filter = Builders<Movie>.Filter.Eq(m => m.Id, id);
            var updateDefiniton = Builders<Movie>.Update
            .Set(m => m.Title, updatedMovie.Title)
            .Set(m => m.Director, updatedMovie.Director)
            .Set(m => m.ReleaseYear, updatedMovie.ReleaseYear);

            await _movies.UpdateOneAsync(filter, updateDefiniton);
        }

        public async Task DeleteAsync(string id)
        {
            var result = await _movies.DeleteOneAsync(Movie => Movie.Id == id);
            if (result.DeletedCount == 0)
            {
                throw new Exception("Movie Not Found.");
            }
        }

    }
}


