namespace foodies_api;

    public class Branches
    {
        public int Id { get; set; }
    
        public string StreetAddress { set; get; } = null!;

        public string? State { set; get; }

        public int? Zipcode { set; get; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int Rating { get; set; }

        public double Distance { get; set; }
    }