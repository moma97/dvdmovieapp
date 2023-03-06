using dvdmovieapp.models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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
        //CRUD 
        #region Read


       
        public async Task<Film> GetFilmByID(int id) //skapa en metod för att hämta filmerna, int id är indata parameter
        {
            // hur jag hämtar filmer från databas
            // eller hämta från mitt API 
            // vi vill bygga en ORM Object relational mapper
            //koppla upp oss mot databasen

            string stmt = "select * from film where film_id@id";

            await using var dataSource = NpgsqlDataSource.Create(_connectionString);

            await using var command = dataSource.CreateCommand(stmt);
            command.Parameters.AddWithValue("id", id);
            await using var reader = await command.ExecuteReaderAsync();
            //kopplingsträng
            //i den har vi lösen och anv namn
            Film film = new Film();
            while (await reader.ReadAsync())
            {
                film = new Film()
                {

                    Film_id = reader.GetInt32(0),
                    Title = (string)reader["title"]

                };
            }

            return film;


        }
        #endregion

        public async Task<IEnumerable<Film>> GetFilms()
        {
            List<Film> films = new List<Film>();
            // Koppla upp mot databasen
            string stmt = "select * from film";
            await using var dataSource = NpgsqlDataSource.Create(_connectionString);

            await using var command = dataSource.CreateCommand(stmt);
            await using var reader = await command.ExecuteReaderAsync();
            Film film = new Film();
            while (await reader.ReadAsync())
            {
                film = new Film()
                {
                    Film_id = reader.GetInt32(0),
                    Title = (string)reader["title"],
                    //Language = new()
                    //{
                    //    Language_id= reader.GetInt32(1),
                    //    Name = (string)reader["name"]   
                    //}
                };
                films.Add(film);
            }

            return films;
        }






        public async Task AddCategory(Category category)
        {
            {// Ni får aldrig skicka in parametrar på detta sätt i en databas!
                try
                {
                    string stmt = $"insert into category(name) values ({category.Name})";

                    stmt = "insert into category(name) values(@name)";
                    await using var dataSource = NpgsqlDataSource.Create(_connectionString);

                    await using var command = dataSource.CreateCommand(stmt);
                    command.Parameters.AddWithValue("name", category.Name);
                    await command.ExecuteNonQueryAsync();

                }
                catch (PostgresException ex)
                {
                    string message = "Det blev fel i databasen. Testa igen";
                    string errorCode = ex.SqlState;
                    //if (errorCode == "22001") 
                    //{
                    //    message = "Namnet är för långt. Max 25 tecken";
                    //}

                    switch (errorCode)
                    {
                        case PostgresErrorCodes.StringDataRightTruncation:
                            message = "Namnet är för långt. Max 25 tecken";
                            break;

                        case PostgresErrorCodes.UniqueViolation:
                            message = "Namnet på kategorin måste vara unikt.";
                            break;
                        default:
                            break;

                    }

                    throw new Exception(message, ex);
                }
            }
        }

        public async Task<Category> AddCategory2(Category category) //<Category> betyder utdata parameter
        {
            {// Ni får aldrig skicka in parametrar på detta sätt i en databas!
                try
                {
                    string stmt = $"insert into category(name) values ({category.Name})";

                    stmt = "insert into category(name) values(@name)";
                    await using var dataSource = NpgsqlDataSource.Create(_connectionString);

                    await using var command = dataSource.CreateCommand(stmt);
                    command.Parameters.AddWithValue("name", category.Name);
                    category.Category_id =(int) await command.ExecuteScalarAsync();
                    return category;

                }
                catch (PostgresException ex)
                {
                    string message = "Det blev fel i databasen. Testa igen";
                    string errorCode = ex.SqlState;
                    //if (errorCode == "22001") 
                    //{
                    //    message = "Namnet är för långt. Max 25 tecken";
                    //}

                    switch (errorCode)
                    {
                        case PostgresErrorCodes.StringDataRightTruncation:
                            message = "Namnet är för långt. Max 25 tecken";
                            break;

                        case PostgresErrorCodes.UniqueViolation:
                            message = "Namnet på kategorin måste vara unikt.";
                            break;
                        default:
                            break;

                    }

                    throw new Exception(message, ex);
                }
            }
        }

    }

}




















