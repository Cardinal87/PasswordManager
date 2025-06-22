using Models;
using Interfaces;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Services.Http
{
    public class HttpWebSiteDataConnector : IHttpDataConnector<WebSiteModel>
    {
        private HttpClient _client;
        private ITokenHandlerService _token;

        public HttpWebSiteDataConnector(IHttpClientFactory factory, ITokenHandlerService token)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
            _token = token;
        }


        async public Task Delete(int id)
        {
            ValidToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/websites/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<WebSiteModel>> GetList()
        {
            ValidToken();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/websites/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var webSites = await response.Content.ReadFromJsonAsync<List<WebSiteModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return webSites;
            
        }

        async public Task<int> Post(WebSiteModel model)
        {
            ValidToken();

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/websites/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            request.Content = content;
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var responsed_model = await response.Content.ReadFromJsonAsync<WebSiteModel>();
            if (responsed_model == null)
                throw new NullReferenceException("no valid json was received from the server");
            return responsed_model.Id;
        }

        async public Task Put(WebSiteModel model, int id)
        {
            ValidToken();

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/websites/{id}");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
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
        private void ValidToken()
        {
            if (_token.IsExpired)
            {
                _token.HandleExpirate();
                throw new UnauthorizedAccessException("Token expired");
            }

        }
    }
}
