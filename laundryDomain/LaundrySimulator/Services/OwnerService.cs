using laundryHeart.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaundrySimulator.Services
{
    internal class OwnerService
    {
        private readonly HttpClient _httpClient;

        public OwnerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Proprietaire>> GetOwnersAsync()
        {
            var response = await _httpClient.GetAsync("Configuration/GetConfiguration");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Proprietaire>>(json);
        }
    }
}
