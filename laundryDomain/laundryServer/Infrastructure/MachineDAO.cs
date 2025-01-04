using laundryHeart.Domain.Entities;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using laundryServer.Business.Interfaces;

namespace laundryServer.Infrastructure
{
    public class MachineDAO : IMachineDAO
    {
        private readonly string _connectionString;

        // Constructeur pour l'injection de dépendance de la chaîne de connexion
        public MachineDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Récupérer toutes les machines
        public List<Machine> GetMachines()
        {
            var machines = new List<Machine>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM machine";  // Requête pour récupérer toutes les machines
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            machines.Add(new Machine
                            {
                                Id = reader.GetInt32("idM"),
                                Name = reader.GetString("type"),
                                IsAvailable = reader.GetBoolean("status"),
                            });
                        }
                    }
                }
            }

            return machines;
        }

        // Récupérer une machine par son ID
        public Machine GetMachineById(int id)
        {
            Machine machine = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM machine WHERE idM = @id";  // Requête pour récupérer une machine par son ID
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            machine = new Machine
                            {
                                Id = reader.GetInt32("idM"),
                                Name = reader.GetString("type"),
                                IsAvailable = reader.GetBoolean("status"),
                            };
                        }
                    }
                }
            }

            return machine;
        }

        // Mettre à jour le statut d'une machine
        public void UpdateMachineStatus(int id, bool isAvailable)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE machine SET status = @status WHERE idM = @id";  // Requête pour mettre à jour le statut
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", isAvailable);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
