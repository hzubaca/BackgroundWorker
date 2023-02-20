using FlowerSpot.SharedKernel.Contracts;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace FlowerSpot.SharedKernel.Services;
public class FlowerSpotHttpClient : IFlowerSpotHttpClient
{
    private readonly HttpClient _client = new HttpClient();

    public FlowerSpotHttpClient(string baseUrl)
    {
        if (_client.BaseAddress == null)
        {
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        }
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        var clientResponse = await _client.GetAsync(url).ConfigureAwait(false);

        if (clientResponse == null || clientResponse.Content == null || (!clientResponse.IsSuccessStatusCode && clientResponse.StatusCode != HttpStatusCode.BadRequest && clientResponse.StatusCode != HttpStatusCode.NotFound))
        {
            throw new Exception("Could not invoke serivcie.");
        }

        var response = await clientResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonConvert.DeserializeObject<T>(response);
    }
}
