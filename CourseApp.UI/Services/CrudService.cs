using CourseApp.UI.Exceptions;
using CourseApp.UI.Models;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CourseApp.UI.Services
{
    public class CrudService : ICrudService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;
        private const string baseUrl = "https://localhost:44392/api/";

        public CrudService(IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];
            if (token!=null)
            {
                if (_client.DefaultRequestHeaders.Contains(HeaderNames.Authorization))
                {
                    _client.DefaultRequestHeaders.Remove(HeaderNames.Authorization);
                }
                _client.DefaultRequestHeaders.Add(HeaderNames.Authorization, token);
            }
        }

        public async Task<PaginatedResponse<TResponse>> GetAllPaginated<TResponse>(string path, int page)
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"{baseUrl}{path}?page={page}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaginatedResponse<TResponse>>(content, options);
            }
            else
            {
                throw new HttpException(response.StatusCode);
            }
        }

        public async Task<TResponse> Get<TResponse>(string path)
        {
            AddAuthorizationHeader();
            var response = await _client.GetAsync($"{baseUrl}{path}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(content, options);
            }
            else
            {
                throw new HttpException(response.StatusCode);
            }
        }

        public async Task<TResponse> Create<TRequest, TResponse>(string path, TRequest data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseUrl}{path}", content);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, options);
            }
            else
            {
                throw new HttpException(response.StatusCode);
            }
        }

        public async Task<TResponse> Edit<TRequest, TResponse>(string path, TRequest data)
        {
            AddAuthorizationHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"{baseUrl}{path}", content);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseContent, options);
            }
            else
            {
                throw new HttpException(response.StatusCode);
            }
        }

        public async Task Delete(string path)
        {
            AddAuthorizationHeader();
            var response = await _client.DeleteAsync($"{baseUrl}{path}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException(response.StatusCode);
            }
        }
    }
}
