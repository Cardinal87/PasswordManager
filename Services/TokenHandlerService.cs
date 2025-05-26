using System.Net;
using System.Net.Http.Headers;

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
            _client.BaseAddress = new Uri("http://localhost:8000/");
        }


        async public Task FetchToken(string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/tokens/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", password);
            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var token = await response.Content.ReadAsStringAsync();
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
