using Interfaces;
using Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Services
{
    public class HttpCardDataConnector : IHttpDataConnector<CardModel>
    {
        private HttpClient _client;
        private TokenHandlerService _token;

        public HttpCardDataConnector(IHttpClientFactory factory, TokenHandlerService token)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
            _token = token;
        }


        async public Task Delete(int id)
        {

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/cards/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<CardModel>> GetList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/cards/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var cards = await response.Content.ReadFromJsonAsync<List<CardModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return cards;

        }

        async public Task Post(CardModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            var request = new HttpRequestMessage(HttpMethod.Post, $"api/cards/");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token.Token);
            request.Content = content;
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task Put(CardModel model, int id)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/websites/{id}");
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
