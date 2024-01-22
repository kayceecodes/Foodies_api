namespace foodies_api;


interface IExternalApiClient {
    Task<string> GetBusiness();
}

public class ExternalApiClient
{
    private readonly HttpClient _httpClient;

    public ExternalApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetBusiness()
    {
        // Make a GET request to the external API
        HttpResponseMessage response = await _httpClient.GetAsync("/businesses/search?term=Burger");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return $"Error: {response.StatusCode}";
    }
}
