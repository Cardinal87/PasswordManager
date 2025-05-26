using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class TokenHandlerService
    {
        private string? token;
        
        public string? Token
        {
            get
            {
                lock (_lock)
                {
                    return token;
                }
            }
            private set
            {
                lock (_lock)
                {
                    token = value;
                }
            }
        }
            private HttpClient _client;
        private object _lock = new object();
        
        public TokenHandlerService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5167/");
        }


        async public Task FetchToken(string password)
        {

            var bytes = Encoding.UTF8.GetBytes(password);
            var encodedPassword = Convert.ToBase64String(bytes);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/tokens/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedPassword);
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            token = json.GetProperty("token").GetString();
            Token = token;
        }

        async private Task HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new HttpRequestException($"Invalid request: {content}", null, HttpStatusCode.BadRequest);
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException($"Server error: {content}", null, HttpStatusCode.InternalServerError);
            }
            else
            {
                throw new HttpRequestException($"Unexpected error: {content}", null, response.StatusCode);
            }
        }
    }
}
