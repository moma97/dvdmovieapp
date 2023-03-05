using dvdmovieapp.models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace dvdmovieapp.DAL
{
    public class DbRepository
    {
        private readonly string _connectionString;


        public DbRepository()
        {
       

            var config = new ConfigurationBuilder().AddUserSecrets<DbRepository>().Build();
            _connectionString = config.GetConnectionString("develop");


        }
        public async Task<Film> GetFilm() //skapa en metod för att hämta filmerna
        {
            // hur jag hämtar filmer från databas
            // eller hämta från mitt API 
            // vi vill bygga en ORM Object relational mapper
            //koppla upp oss mot databasen

            string stmt = "select * from film where film_id=1";

            await using var dataSource = NpgsqlDataSource.Create(_connectionString);

            await using var command = dataSource.CreateCommand(stmt);

            await using var reader = await command.ExecuteReaderAsync();
            //kopplingsträng
            //i den har vi lösen och anv namn
            Film film = new Film();
            while(await reader.ReadAsync())
            {
                film = new Film()
                {

                    Film_id = reader.GetInt32(0),
                    Title = (string)reader["title"]

                };
            }

            return film; 


        }

    }
}
