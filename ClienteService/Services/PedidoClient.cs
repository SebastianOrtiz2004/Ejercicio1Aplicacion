using ClienteService.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;


namespace ClienteService.Services
{
    public class PedidoClient
    {
        private readonly HttpClient _http;
        public PedidoClient(HttpClient http) { _http = http; }

        public async Task<string> GetTokenAsync(string username, string password)
        {
            var resp = await _http.PostAsJsonAsync("api/auth/login", new { username, password });
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadFromJsonAsync<JsonElement>();
            if (json.TryGetProperty("token", out var tokenElem))
                return tokenElem.GetString();
            return null;
        }

        public async Task<PedidoDto> CreatePedidoAsync(PedidoDto pedido, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/pedidos");
            request.Content = JsonContent.Create(pedido);
            if (!string.IsNullOrEmpty(token)) request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resp = await _http.SendAsync(request);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<PedidoDto>();
        }

        public async Task<PedidoDto> GetPedidoAsync(int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/pedidos/{id}");
            if (!string.IsNullOrEmpty(token)) request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var resp = await _http.SendAsync(request);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<PedidoDto>();
        }
    }
}
