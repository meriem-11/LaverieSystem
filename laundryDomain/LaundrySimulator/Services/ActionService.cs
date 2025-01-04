using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LaundrySimulator.Services
{
    internal class ActionService
    {
        private readonly HttpClient _httpClient;

        public ActionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SaveActionAsync(Action action)
        {
            var json = JsonSerializer.Serialize(action);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Actions", content);
            response.EnsureSuccessStatusCode();
        }
}
}
