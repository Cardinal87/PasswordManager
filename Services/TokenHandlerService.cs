using Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class TokenHandlerService : ITokenHandlerService
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
        public bool IsExpired
        {
            get
            {
                if (String.IsNullOrEmpty(token))
                {
                    return true;
                }
                var handler = new JwtSecurityTokenHandler();
                var decodedToken = handler.ReadJwtToken(token);
                return DateTime.UtcNow > decodedToken.ValidTo;
            }
        }
        public event Action? TokenExpired;
        private HttpClient _client;
        private object _lock = new object();
        
        public TokenHandlerService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5167/");
        }


        async public Task<HttpStatusCode> FetchToken(string password)
        {

            var bytes = Encoding.UTF8.GetBytes(password);
            var encodedPassword = Convert.ToBase64String(bytes);
            var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/tokens/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedPassword);
            var response = await _client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return HttpStatusCode.Unauthorized;
            }
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            token = json.GetProperty("token").GetString();
            Token = token;
            return HttpStatusCode.OK;
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

        public void HandleExpirate()
        {
            TokenExpired?.Invoke();
        }
    }
}
