using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace KuranGuide.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public ApiClient(HttpClient client, IConfiguration config, IHttpContextAccessor accessor)
        {
            _client = client;
            _client.BaseAddress = new Uri(config["ApiBaseUrl"]);
            _httpContextAccessor = accessor;
        }

        /// <summary>
        /// JWT token'i cookie'den alir ve bir HttpRequestMessage'a ekler.
        /// DefaultRequestHeaders'i degistirmez — thread-safe.
        /// </summary>
        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, bool withAuth = false)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (withAuth)
            {
                var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return request;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string endpoint, string token)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return request;
        }

        // --------------------------------------------------------
        // GET (Anonim)
        // --------------------------------------------------------
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var request = CreateRequest(HttpMethod.Get, endpoint);
            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        public async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            var result = await GetAsync<List<T>>(endpoint);
            return result ?? new List<T>();
        }

        // --------------------------------------------------------
        // GET (Auth)
        // --------------------------------------------------------
        public async Task<T?> GetWithAuthAsync<T>(string endpoint, string token)
        {
            var request = CreateRequest(HttpMethod.Get, endpoint, token);
            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }

        // --------------------------------------------------------
        // POST (Anonim)
        // --------------------------------------------------------
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint);
            request.Content = new StringContent(
                JsonSerializer.Serialize(body, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseJson, _jsonOptions);
        }

        // --------------------------------------------------------
        // POST (Auth)
        // --------------------------------------------------------
        public async Task<TResponse?> PostWithAuthAsync<TRequest, TResponse>(string endpoint, TRequest body, string token)
        {
            var request = CreateRequest(HttpMethod.Post, endpoint, token);
            request.Content = new StringContent(
                JsonSerializer.Serialize(body, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseJson, _jsonOptions);
        }

        // --------------------------------------------------------
        // PUT (Auth)
        // --------------------------------------------------------
        public async Task<TResponse?> PutWithAuthAsync<TRequest, TResponse>(string endpoint, TRequest body, string token)
        {
            var request = CreateRequest(HttpMethod.Put, endpoint, token);
            request.Content = new StringContent(
                JsonSerializer.Serialize(body, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseJson, _jsonOptions);
        }

        // --------------------------------------------------------
        // DELETE (Auth)
        // --------------------------------------------------------
        public async Task<bool> DeleteWithAuthAsync(string endpoint, string token)
        {
            var request = CreateRequest(HttpMethod.Delete, endpoint, token);
            var response = await _client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
