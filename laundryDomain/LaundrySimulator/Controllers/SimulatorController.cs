using LaundrySimulator.Services;
using LaundrySimulator.UserInterface;
using System;
using System.Threading.Tasks;
using laundryHeart.Domain.Entities;
using laundryHeart;

namespace LaundrySimulator.Controllers
{
    internal class SimulatorController
    {
        private readonly OwnerService _ownerService;
        private readonly ActionService _actionService;
        private readonly MachineService _machineService;

        public SimulatorController(OwnerService ownerService, ActionService actionService, MachineService machineService)
        {
            _ownerService = ownerService;
            _actionService = actionService;
            _machineService = machineService;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("Welcome to the Laundry Simulator!");

            try
            {
                // Step 1: Fetch and display owners
                var owners = await _ownerService.GetOwnersAsync();
                if (owners.Count == 0)
                {
                    Console.WriteLine("No owners available.");
                    return;
                }

                var ownerIndex = ConsoleHelper.DisplayMenu(owners, o => o.Name);
                var selectedOwner = owners[ownerIndex];

                // Step 2: Select laundry
                var laundries = selectedOwner.Laveries;
                if (laundries.Count == 0)
                {
                    Console.WriteLine("No laundries available for the selected owner.");
                    return;
                }

                var laundryIndex = ConsoleHelper.DisplayMenu(laundries, l => l.Name);
                var selectedLaundry = laundries[laundryIndex];

                // Step 3: Select machine
                var machines = selectedLaundry.Machines;
                if (machines.Count == 0)
                {
                    Console.WriteLine("No machines available in the selected laundry.");
                    return;
                }

                var machineIndex = ConsoleHelper.DisplayMenu(machines, m => $"{m.Name} - Status: {(m.IsAvailable ? "Available" : "In Use")}");
                var selectedMachine = machines[machineIndex];

                // Check if the machine is available
                if (!selectedMachine.IsAvailable)
                {
                    Console.WriteLine("The selected machine is currently in use. Please try again later.");
                    return;
                }

                // Step 4: Select cycle
                var cycles = selectedMachine.Cycles;
                if (cycles.Count == 0)
                {
                    Console.WriteLine("No cycles available for the selected machine.");
                    return;
                }

                var cycleIndex = ConsoleHelper.DisplayMenu(cycles, c => $"{c.Id} - Type: {c.Type} - Duration: {c.Duration} - Cost: {c.Cost}");
                var selectedCycle = cycles[cycleIndex];

                Console.WriteLine($"Starting Cycle {selectedCycle.Id} on Machine {selectedMachine.Name}...");

                // Step 5: Update machine status to "In Use"
                await _machineService.UpdateMachineStatusAsync(selectedMachine.Id, "In Use");
                selectedMachine.IsAvailable = false;

                // Simulate timer for the cycle duration
                var durationInMilliseconds = (int)selectedCycle.Duration.TotalMilliseconds;
                await Task.Delay(durationInMilliseconds);

                // Step 6: Save the action
                var action = new LaundrySimulator.Action
                {
                    StartTime = DateTime.Now,

                    EndTime = DateTime.Now.AddMilliseconds(durationInMilliseconds),
                    CycleId = selectedCycle.Id,
                };

                await _actionService.SaveActionAsync(action);

                // Step 7: Update machine status to "Available"
                await _machineService.UpdateMachineStatusAsync(selectedMachine.Id, "Available");
                selectedMachine.IsAvailable = true;

                Console.WriteLine("Cycle Completed and Action Saved!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
