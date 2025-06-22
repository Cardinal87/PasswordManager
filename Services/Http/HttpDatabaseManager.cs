using Newtonsoft.Json;
using System.Net;

namespace Services.Http
{
    public class HttpDatabaseManager
    {
        private HttpClient _client;

        public HttpDatabaseManager(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/api/");
        }


        public bool IsDatabaseExist()
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"database");
            var response = _client.Send(message);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;

        }

        public async Task CreateDatabase(string password)
        {
            var json = JsonConvert.SerializeObject(new { password });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"database", content);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        public async Task DeleteDatabase()
        {
            var response = await _client.DeleteAsync($"database");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
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
