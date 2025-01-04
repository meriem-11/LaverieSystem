using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaundrySimulator.Services
{
    internal class MachineService
    {
        private readonly HttpClient _httpClient;

        public MachineService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UpdateMachineStatusAsync(int machineId, string status)
        {
            var json = JsonSerializer.Serialize(new { MachineId = machineId, Status = status });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Machines/{machineId}/status", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
