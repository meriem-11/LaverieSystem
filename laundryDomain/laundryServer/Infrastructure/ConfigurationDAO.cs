using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;
using MySql.Data.MySqlClient;

namespace laundryServer.Infrastructure
{
    public class ConfigurationDAO : IConfigurationDAO
    {    
        private readonly string _connectionString;

        public List<Proprietaire> Configuration => throw new NotImplementedException();

        public ConfigurationDAO(IConfiguration Configuration)
    {
        _connectionString = Configuration.GetConnectionString("DefaultConnection");
    }

    // Example method to get data from the database
    public List<Proprietaire> GetConfiguration()
    {
        var entities = new List<Proprietaire>();

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            var command = new MySqlCommand("SELECT * FROM proprietaire", connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var entity = new Proprietaire
                    {
                        Id = reader.GetInt32("idProprietaire"),
                        Name = reader.GetString("Nom"),
                        // Map other columns here
                    };
                    entities.Add(entity);
                }
            }
        }

        return entities;
    }

    // Example method to execute a query
    public void ExecuteQuery(string query)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            var command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
    }


    
}

