using Models;
using Interfaces;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Services
{
    public class HttpWebSiteDataConnector : IHttpDataConnector<WebSiteModel>
    {
        private HttpClient _client;

        public HttpWebSiteDataConnector(IHttpClientFactory factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri(@"http://localhost:5167/");
        }
        
        
        async public Task Delete(int id)
        {
            
            using var response = await _client.DeleteAsync($"api/websites/{id}");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task<List<WebSiteModel>> GetList()
        {
            using var response = await _client.GetAsync(@"api/websites/");
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
            var webSites = await response.Content.ReadFromJsonAsync<List<WebSiteModel>>() ?? throw new InvalidDataException("Failed to deserialize data");
            return webSites;
            
        }

        async public Task Post(WebSiteModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);
            
            using var response = await _client.PostAsync("api/webites/", content);
            if (!response.IsSuccessStatusCode)
            {
                await HandleResponse(response);
            }
        }

        async public Task Put(WebSiteModel model, int id)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json);

            using var response = await _client.PutAsync($"api/webites/{id}", content);
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
