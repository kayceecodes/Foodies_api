namespace foodies_api;

public class SearchDto
{
    public string Location { get; set; }

    public string PhoneNumber { get; set; }

    public string Name { get; set; }
    
    public List<string> Terms { get; set; } 
}
