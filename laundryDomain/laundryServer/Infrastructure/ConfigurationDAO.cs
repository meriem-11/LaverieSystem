using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace laundryServer.Infrastructure
{
    public class ConfigurationDAO : IConfigurationDAO
    {
        private readonly string _connectionString;

        public ConfigurationDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Dictionary<string, object> GetConfiguration()
        {
            var result = new Dictionary<string, object>();

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    result["proprietaires"] = GetProprietairesWithDetails(connection);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Une erreur s'est produite lors de l'accès à la base de données : {ex.Message}", ex);
            }

            return result;
        }

        private List<Proprietaire> GetProprietairesWithDetails(MySqlConnection connection)
        {
            var proprietaires = new List<Proprietaire>();

            const string query = @"
        SELECT 
            p.idProprietaire, 
            p.Nom AS ProprietaireNom, 
            l.idL AS LaverieId, 
            l.Nom AS LaverieNom,
            m.idM AS MachineId,
            m.type AS MachineType,
            m.status AS MachineStatus,
            c.id AS CycleId,
            c.cout AS CycleCout,
            c.duree AS CycleDuree,
            a.id AS ActionId,
            a.date AS ActionDate
        FROM 
            proprietaire p
        LEFT JOIN 
            laverie l ON p.idProprietaire = l.idProprietaire
        LEFT JOIN 
            machine m ON l.idL = m.idL
        LEFT JOIN 
            cycle c ON m.idM = c.idM
        LEFT JOIN 
            action a ON c.id = a.idC";

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // 1. Propriétaire
                        var proprietaireId = reader.GetInt32("idProprietaire");
                        var proprietaire = proprietaires.FirstOrDefault(p => p.Id == proprietaireId);
                        if (proprietaire == null)
                        {
                            proprietaire = new Proprietaire
                            {
                                Id = proprietaireId,
                                Name = reader.GetString("ProprietaireNom"),
                                Laveries = new List<Laverie>()
                            };
                            proprietaires.Add(proprietaire);
                        }

                        // 2. Laverie
                        if (!reader.IsDBNull(reader.GetOrdinal("LaverieId")))
                        {
                            var laverieId = reader.GetInt32("LaverieId");
                            var laverie = proprietaire.Laveries.FirstOrDefault(l => l.Id == laverieId);
                            if (laverie == null)
                            {
                                laverie = new Laverie
                                {
                                    Id = laverieId,
                                    Name = reader.GetString("LaverieNom"),
                                    Machines = new List<Machine>()
                                };
                                proprietaire.Laveries.Add(laverie);
                            }

                            // 3. Machine
                            if (!reader.IsDBNull(reader.GetOrdinal("MachineId")))
                            {
                                var machineId = reader.GetInt32("MachineId");
                                var machine = laverie.Machines.FirstOrDefault(m => m.Id == machineId);
                                if (machine == null)
                                {
                                    machine = new Machine
                                    {
                                        Id = machineId,
                                        Name = reader.GetString("MachineType"),
                                        IsAvailable = reader.GetBoolean("MachineStatus"),
                                        Earnings = 0,
                                        Cycles = new List<Cycle>()
                                    };
                                    laverie.Machines.Add(machine);
                                }

                                // 4. Cycle
                                if (!reader.IsDBNull(reader.GetOrdinal("CycleId")))
                                {
                                    var cycleId = reader.GetInt32("CycleId");
                                    var cycle = machine.Cycles.FirstOrDefault(c => c.Id == cycleId);
                                    if (cycle == null)
                                    {
                                        cycle = new Cycle
                                        {
                                            Id = cycleId,
                                            Cost = reader.GetInt32("CycleCout"),
                                            Duration = reader.GetTimeSpan("CycleDuree"),
                                            Actions = new List<laundryHeart.Domain.Entities.Action>()
                                        };
                                        machine.Cycles.Add(cycle);
                                    }

                                    // 5. Action
                                    if (!reader.IsDBNull(reader.GetOrdinal("ActionId")))
                                    {
                                        var actionId = reader.GetInt32("ActionId");
                                        if (!cycle.Actions.Any(a => a.Id == actionId))
                                        {
                                            cycle.Actions.Add(new laundryHeart.Domain.Entities.Action
                                            {
                                                Id = actionId,
                                                Date = reader.GetDateTime("ActionDate")
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return proprietaires;
        }



    }
}

