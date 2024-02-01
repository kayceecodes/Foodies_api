namespace foodies_api.Models;

    public class Category
    {
        public string? Alias { get; set; }
        public string? Title { get; set; }
    }
 
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
 
    public class Location
    {
        public string Address1 { get; set; } = null!;
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string City { get; set; } = null!;
        public string Zip_code { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? State { get; set; }
        public List<string> Display_address { get; set; } = null!;
    }
 
    public class Business
    {
        public string Id { get; set; }
        public string? Alias { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Image_url { get; set; }
        public bool Is_closed { get; set; }
        public string? Url { get; set; }
        public int Review_count { get; set; }
        public List<Category> Categories { get; set; } = null!;
        public double Rating { get; set; }
        public Coordinates? Coordinates { get; set; }
        public List<string>? Transactions { get; set; }
        public string? Price { get; set; }
        public Location? Location { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        public string? Display_phone { get; set; } = null!;
        public double Distance { get; set; }
    }
 
    public class Center
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
 
    public class Region
    {
        public Center? Center { get; set; }
    }
 
    public class YelpResponse
    {
        public List<Business>? Businesses { get; set; }
        public int Total { get; set; }
        public Region? Region { get; set; }
    }
