using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace frontend_task_2.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
     private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
                           _httpClient.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? "https://cmsback.sampaarsh.cloud");
            _httpContextAccessor = httpContextAccessor;
            //(Case shouldn't be sensitive here)
            _jsonOptions = new JsonSerializerOptions
            {
                                   PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        private void AddAuthToken()
        {
                       var token = _httpContextAccessor.HttpContext?.User?.FindFirst("Token")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                //  (Token is added here brothers)
                              _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            AddAuthToken();
                     var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                             return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            return default;
        }


        public async Task<(TResponse? Data, string? Error)> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            AddAuthToken();
            var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
       var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonSerializer.Deserialize<TResponse>(responseContent, _jsonOptions);
                return (responseData, null);
            }
                // (Get error msg if fails)
                try
                {
                    var errObj = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent, _jsonOptions);
            if (errObj != null && errObj.ContainsKey("error"))
                    {
                        return (default, errObj["error"]);
                    }
                }
                { 
                    // If we can't parse it as a dict, just return the raw string to see what the server is unhappy about
                    return (default, $"API returned: {response.StatusCode} - {responseContent}");
                }
                return (default, $"Something went wrong. Raw: {responseContent}");
        }
        
        public async Task<(bool Success, string? Error)> PatchAsync<TRequest>(string endpoint, TRequest data)
        {
            AddAuthToken();
          
            
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            
            
      var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
            {
                Content = content
            };

       var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            
      var responseContent = await response.Content.ReadAsStringAsync();
            try
            {
         var errObj = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent, _jsonOptions);
                if (errObj != null && errObj.ContainsKey("error"))
                {
              return (false, errObj["error"]);
                }
            }
            catch 
            { 
               return (false, $"API returned: {response.StatusCode} - {responseContent}");
            }
            return (false, $"Something went wrong. Raw: {responseContent}");
        }
    }
}
