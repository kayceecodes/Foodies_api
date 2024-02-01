using System.Net;

namespace foodies_api.Models.Dtos;

public class APIResponse
{
    public APIResponse()
    {
        ErrorMessages = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public Object Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; }
}
