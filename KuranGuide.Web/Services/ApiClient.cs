using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace KuranGuide.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(HttpClient client, IConfiguration config, IHttpContextAccessor accessor)
        {
            _client = client;
            _client.BaseAddress = new Uri(config["ApiBaseUrl"]);

            _httpContextAccessor = accessor;
        }
        private HttpClient CreateClient(bool withAuth = false)
        {
            // Zaten var olan client'ı kullanıyoruz.
            var client = _client;

            // Her çağrıda önce eski Authorization temizlenmeli
            client.DefaultRequestHeaders.Authorization = null;

            // Token gerekliyse ekle
            if (withAuth)
            {
                var token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return client;
        }

        public async Task<List<T>> GetListAsync<T>(string endpoint)
        {
            var result = await GetAsync<List<T>>(endpoint);
            return result ?? new List<T>();
        }

        // --------------------------------------------------------
        // GET (Anonim)
        // --------------------------------------------------------
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var client = CreateClient();
            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                // Hata mesajını oku ve konsola yaz veya breakpoint koy
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Hatası: {errorContent}");
                return default;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Büyük/Küçük harf takıntısını kaldırır
            };
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options);
        }

        // --------------------------------------------------------
        // GET (Auth)
        // --------------------------------------------------------
        public async Task<T?> GetWithAuthAsync<T>(string endpoint, string token)
        {
            var client = CreateClient(withAuth: false);

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }


        // --------------------------------------------------------
        // POST (Anonim)
        // --------------------------------------------------------
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var client = CreateClient();

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        // --------------------------------------------------------
        // POST (Auth)
        // --------------------------------------------------------
        public async Task<TResponse?> PostWithAuthAsync<TRequest, TResponse>(string endpoint, TRequest body, string token)
        {
            var client = CreateClient(withAuth: true);

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }

        public async Task<TResponse?> PutWithAuthAsync<TRequest, TResponse>(string endpoint, TRequest body, string token)
        {
            var client = CreateClient(withAuth: true);

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
                return default;

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }


        // --------------------------------------------------------
        // DELETE (Auth)
        // --------------------------------------------------------
        public async Task<bool> DeleteWithAuthAsync(string endpoint, string token)
        {
            var client = CreateClient(withAuth: true);

            var response = await client.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }
}
