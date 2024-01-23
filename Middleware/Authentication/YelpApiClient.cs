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

    public YelpApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResult<string>> GetBusiness()
    {

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/data");

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
