using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaundrySimulator
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Simulateur de Laverie ===");
                Console.WriteLine("1. Afficher la configuration complète");
                Console.WriteLine("2. Quitter");
                Console.Write("Votre choix : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await DisplayConfiguration();
                        break;
                    case "2":
                        Console.WriteLine("Merci d'avoir utilisé le simulateur !");
                        return;
                    default:
                        Console.WriteLine("Choix invalide. Appuyez sur une touche pour réessayer...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task DisplayConfiguration()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Configuration Complète ===");

                // Appel à l'API GetConfiguration
                string url = "https://localhost:7148/api/Configuration/GetConfiguration";
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erreur : Impossible de récupérer la configuration (Code {response.StatusCode})");
                    Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                    return;
                }

                string responseContent = await response.Content.ReadAsStringAsync();
                var jsonData = JsonDocument.Parse(responseContent).RootElement;

                if (!jsonData.TryGetProperty("proprietaires", out var proprietaires) || proprietaires.ValueKind != JsonValueKind.Array)
                {
                    Console.WriteLine("Erreur : La clé 'proprietaires' est manquante ou non formatée comme tableau.");
                    Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                    return;
                }

                // Affichage des données des propriétaires et de leurs laveries
                var selectedProprietaire = await SelectProprietaire(proprietaires);

                if (selectedProprietaire != null)
                {
                    var selectedLaverie = await SelectLaverie((JsonElement)selectedProprietaire);
                    if (selectedLaverie != null)
                    {
                        var selectedMachine = await SelectMachine((JsonElement)selectedLaverie);
                        if (selectedMachine != null)
                        {
                            var selectedCycle = await SelectCycle((JsonElement)selectedMachine);
                            if (selectedCycle != null)
                            {
                                await StartMachine((JsonElement)selectedCycle, (JsonElement)selectedMachine);
                            }
                        }
                    }

                }

                Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erreur d'analyse JSON : {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erreur de requête HTTP : {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur inattendue s'est produite : {ex.Message}");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
            }
        }

        private static async Task<JsonElement?> SelectProprietaire(JsonElement proprietaires)
        {
            Console.WriteLine("Sélectionnez un propriétaire :");
            int i = 1;
            foreach (var proprietaire in proprietaires.EnumerateArray())
            {
                Console.WriteLine($"{i}. {proprietaire.GetProperty("name").GetString()}");
                i++;
            }

            int choice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (choice >= 0 && choice < proprietaires.GetArrayLength())
            {
                return proprietaires[choice];
            }
            return null;
        }

        private static async Task<JsonElement?> SelectLaverie(JsonElement proprietaire)
        {
            var laveries = proprietaire.GetProperty("laveries");

            Console.WriteLine("Sélectionnez une laverie :");
            int i = 1;
            foreach (var laverie in laveries.EnumerateArray())
            {
                Console.WriteLine($"{i}. {laverie.GetProperty("name").GetString()}");
                i++;
            }

            int choice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (choice >= 0 && choice < laveries.GetArrayLength())
            {
                return laveries[choice];
            }
            return null;
        }

        private static async Task<JsonElement?> SelectMachine(JsonElement laverie)
        {
            var machines = laverie.GetProperty("machines");

            Console.WriteLine("Sélectionnez une machine :");
            int i = 1;
            foreach (var machine in machines.EnumerateArray())
            {
                Console.WriteLine($"{i}. {machine.GetProperty("name").GetString()} (Statut : {(machine.GetProperty("isAvailable").GetBoolean() ? "Disponible" : "Indisponible")})");
                i++;
            }

            int choice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (choice >= 0 && choice < machines.GetArrayLength())
            {
                return machines[choice];
            }
            return null;
        }

        private static async Task<JsonElement?> SelectCycle(JsonElement machine)
        {
            var cycles = machine.GetProperty("cycles");

            Console.WriteLine("Sélectionnez un cycle :");
            int i = 1;
            foreach (var cycle in cycles.EnumerateArray())
            {
                Console.WriteLine($"{i}. Coût: {cycle.GetProperty("cost").GetDecimal()}€, Durée: {cycle.GetProperty("duration").GetString()}");
                i++;
            }

            int choice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (choice >= 0 && choice < cycles.GetArrayLength())
            {
                return cycles[choice];
            }
            return null;
        }

        private static async Task StartMachine(JsonElement cycle, JsonElement machine)
        {
            bool isAvailable = machine.GetProperty("isAvailable").GetBoolean();

            if (!isAvailable)
            {
                Console.WriteLine("La machine est déjà en cours d'utilisation ou indisponible.");
                Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
                Console.ReadKey();
                return;
            }

            // Change le statut de la machine à "indisponible"
            machine.GetProperty("isAvailable").GetBoolean();  // Juste pour mettre à jour la machine localement
            Console.WriteLine("Démarrage de la machine...");

            // Créer une action pour démarrer la machine
            var startTime = DateTime.Now;
            var duration = cycle.GetProperty("duration").GetString();
            TimeSpan cycleDuration = TimeSpan.Parse(duration);

            var endTime = startTime.Add(cycleDuration);
            Console.WriteLine($"Cycle sélectionné avec durée : {cycleDuration.TotalMinutes} minutes.");
            Console.WriteLine($"Début : {startTime}");
            Console.WriteLine($"Fin prévue : {endTime}");

            // Simule le fonctionnement de la machine
            await RunMachineTimer(cycleDuration);

            // Remet le statut de la machine à "disponible"
            Console.WriteLine("La machine est maintenant terminée et disponible.");
            machine.GetProperty("isAvailable").GetBoolean(); // Met à jour le statut de la machine

            // Crée une action pour ce cycle
            Console.WriteLine("Création de l'action...");
            var action = new
            {
                MachineId = machine.GetProperty("id").GetInt32(),
                CycleId = cycle.GetProperty("id").GetInt32(),
                StartTime = startTime,
                EndTime = endTime
            };

            Console.WriteLine("Action enregistrée avec succès !");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }

        private static async Task RunMachineTimer(TimeSpan duration)
        {
            Console.WriteLine($"La machine fonctionne pendant {duration.TotalMinutes} minutes.");
            await Task.Delay(duration);
            Console.WriteLine("La machine est arrêtée !");
        }
    }
}
