using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Services
{
    public class HttpAppDataConnector : IHttpDataConnector<AppModel>
    {

        private HttpClient _client;

        public HttpAppDataConnector(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
        }

        async public Task Delete(int id)
        {
            using var response = await _client.DeleteAsync($"api/apps/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<AppModel>> GetList()
        {
            using var response = await _client.GetAsync(@"api/apps/");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var apps = await response.Content.ReadFromJsonAsync<List<AppModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return apps;
        }

        async public Task Post(AppModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            using var response = await _client.PostAsync("api/apps/", content);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task Put(AppModel model, int id)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            using var response = await _client.PutAsync($"api/cards/{id}", content);
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
