using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Services
{
    public class HttpAppDataConnector : IHttpDataConnector<AppModel>
    {

        private HttpClient _client;
        private TokenHandlerService _token;

        public HttpAppDataConnector(IHttpClientFactory factory, TokenHandlerService token)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
            _token = token;
        }

        async public Task Delete(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/apps/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<AppModel>> GetList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/apps/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var apps = await response.Content.ReadFromJsonAsync<List<AppModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return apps;
        }

        async public Task<int> Post(AppModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/apps/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            request.Content = content;
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var responsed_model = await response.Content.ReadFromJsonAsync<AppModel>();
            if (responsed_model == null)
                throw new NullReferenceException("no valid json was received from the server");
            return responsed_model.Id;
        }

        async public Task Put(AppModel model, int id)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/apps/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            request.Content = content;
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
    }
}
