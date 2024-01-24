using System.Net.Http.Headers;

namespace foodies_api;

interface IYelpApiClient {
    Task<ApiResult<string>> GetBusiness();
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
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;

    public YelpApiClient(HttpClient httpClient, string accessToken)
    {
        _httpClient = httpClient;
        _accessToken = accessToken;
    }

    public async Task<ApiResult<string>> GetBusiness()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        var Id = "ifEkf8JxP3RCBeszcIGLww";

        try
        {
            // Make a GET request to Yelp Fusion API
            HttpResponseMessage response = await _httpClient.GetAsync($"/businesses/{Id}");
            // HttpResponseMessage response = await _httpClient.GetAsync($"https://api.yelp.com/v3/businesses/{Id}");

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
