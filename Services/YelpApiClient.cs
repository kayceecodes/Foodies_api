using Newtonsoft.Json;
using System.Net.Http.Headers;
using foodies_api.Models;
using foodies_api.Models.Responses.Yelp;
using Microsoft.IdentityModel.Tokens;

namespace foodies_api;

interface IYelpApiClient {
    Task<APIResult> GetBusinessById(string id);
    Task<APIResult> GetBusinessesByLocation(string location);
    Task<APIResult> GetBusinessesByPhone(string phonenumber);
    Task<APIResult> GetBusinesses(SearchDto dto);
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

    public async Task<APIResult> GetBusinessById(string id)
    {
        var token = _configuration.GetValue<string>(YelpConstants.ApiKeySectionName);
        var httpClient = _httpClientFactory.CreateClient("YelpApiClient");
        string url = httpClient.BaseAddress + $"/businesses/{id}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Make a GET request to Yelp Fusion API
        HttpResponseMessage result = await httpClient.GetAsync(url);
        var business = JsonConvert.DeserializeObject<Business>(await result.Content.ReadAsStringAsync());

        if(result.IsSuccessStatusCode)
        {
            return new APIResult() {
                IsSuccess = result.IsSuccessStatusCode,
                Data = business ?? new(),
                StatusCode = result.StatusCode
            };
        }        
        else {
            // Handle exceptions (e.g., network issues)
            return new APIResult() {
                IsSuccess = false,
                StatusCode = result.StatusCode,
                ErrorMessages = new () { result.ReasonPhrase }
            };     
        } 
    }
        
    public async Task<APIResult> GetBusinessesByLocation(string location)
    {
        var token = _configuration.GetValue<string>(YelpConstants.ApiKeySectionName);
        var httpClient = _httpClientFactory.CreateClient("YelpApiClient");
        string url = httpClient.BaseAddress + $"/businesses/search?term=restaurant&sort_by=best_match&limit=20&location={location}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Make a GET request to Yelp Fusion API
        HttpResponseMessage result = await httpClient.GetAsync(url);
        var businesses = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());

        if(result.IsSuccessStatusCode)
        {
            return new APIResult() {
                IsSuccess = result.IsSuccessStatusCode,
                Data = businesses ?? new (),
                // StatusCode = result.StatusCode
            };
        }        
        else {
            return new APIResult() {
                IsSuccess = false,
                Data = new object(),
                StatusCode = result.StatusCode,
                ErrorMessages = new () { result.ReasonPhrase }
            };     
        } 
    }

    public async Task<APIResult> GetBusinessesByPhone(string number)
    {
        var token = _configuration.GetValue<string>(YelpConstants.ApiKeySectionName);
        var httpClient = _httpClientFactory.CreateClient("YelpApiClient");
        string url = httpClient.BaseAddress + $"/businesses/search/phone?sort_by=best_match&phone={number}";
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Make a GET request to Yelp Fusion API
        HttpResponseMessage result = await httpClient.GetAsync(url);
        var businesses = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());

        if(result.IsSuccessStatusCode)
        {
            return new APIResult() {
                IsSuccess = result.IsSuccessStatusCode,
                Data = businesses ?? new (),
                // StatusCode = result.StatusCode
            };
        }        
        else {
            return new APIResult() {
                IsSuccess = false,
                Data = new object(),
                StatusCode = result.StatusCode,
                ErrorMessages = new () { result.ReasonPhrase }
            };     
        } 
    }

    public async Task<APIResult> GetBusinesses(SearchDto dto)
    {
        var token = _configuration.GetValue<string>(YelpConstants.ApiKeySectionName);
        var httpClient = _httpClientFactory.CreateClient("YelpApiClient");
       
        string terms = dto.Terms.Count > 0 ? string.Join(", ", dto.Terms) : "";// add split for terms
        bool IsMissingLatLong = dto.Lat.IsNullOrEmpty() || dto.Long.IsNullOrEmpty();
        
        if(IsMissingLatLong && dto.Location.IsNullOrEmpty())
            throw new NullReferenceException("There no value for Lat, Long, or Location");

        string url = 
          httpClient.BaseAddress + $"/businesses/search?term={terms}&sort_by=best_match&limit={dto.Limit}" + 
          "&location={dto.Location}&latitude={dto.Lat}&longitude={dto.Long}&categories={dto.Category}";
        
        // Make a GET request to Yelp Fusion API
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage result = await httpClient.GetAsync(url);
        var businesses = JsonConvert.DeserializeObject<YelpResponse>(await result.Content.ReadAsStringAsync());

        if(result.IsSuccessStatusCode)
        {
            return new APIResult() {
                IsSuccess = result.IsSuccessStatusCode,
                Data = businesses ?? new (),
                // StatusCode = result.StatusCode
            };
        }        
        else {
            return new APIResult() {
                IsSuccess = false,
                Data = new object(),
                StatusCode = result.StatusCode,
                ErrorMessages = new () { result.ReasonPhrase }
            };     
        } 
    }
}