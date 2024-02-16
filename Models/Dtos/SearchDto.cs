namespace foodies_api;

public class SearchDto
{
    public string Category { get; set; }
    public string Location { get; set; }
    public string Lat { get; set; }
    public string Long { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public List<string> Terms { get; set; } 
    public int Limit { get; set; }

}
