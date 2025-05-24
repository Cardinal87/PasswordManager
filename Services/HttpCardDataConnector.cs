using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;

namespace Services
{
    public class HttpCardDataConnector : IHttpDataConnector<CardModel>
    {
        private HttpClient _client;

        public HttpCardDataConnector(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
        }


        async public Task Delete(int id)
        {

            using var response = await _client.DeleteAsync($"api/cards/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<CardModel>> GetList()
        {
            using var response = await _client.GetAsync(@"api/cards/");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var webSites = await response.Content.ReadFromJsonAsync<List<CardModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return webSites;

        }

        async public Task Post(CardModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            using var response = await _client.PostAsync("api/cards/", content);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task Put(CardModel model, int id)
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
