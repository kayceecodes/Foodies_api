using System.Net.Http.Headers;

namespace foodies_api;

interface IYelpApiClient {
    Task<ApiResult<string>> GetBusiness(string id);
}
public class ApiResult<T>
{
    public T Data { get; set; }
    public bool IsSuccessStatusCode { get; set; }
    public int? StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}
public class YelpApiClient : IYelpApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration; 

    public YelpApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<ApiResult<string>> GetBusiness(string id)
    {
        var token = _configuration.GetValue<string>(AuthConstants.YelpApiKeyName);
        using var httpClient = _httpClientFactory.CreateClient("YelpApiClient");
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        try
        {
            // Make a GET request to Yelp Fusion API
            HttpResponseMessage response = await httpClient.GetAsync($"/businesses/{id}");

            return new ApiResult<string>
            {
                Data = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null,
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                ErrorMessage = !response.IsSuccessStatusCode ? "Failed to fetch data." : null
            };
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., network issues)
            return new ApiResult<string>
            {
                IsSuccessStatusCode = false,
                StatusCode = null,
                ErrorMessage = $"Exception: {ex.Message}"
            };
        }

    }
}
