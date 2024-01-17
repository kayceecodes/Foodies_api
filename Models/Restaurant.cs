using System.ComponentModel.DataAnnotations;
using foodies_api.Models;

namespace foodies_api;

public class Restaurant
{
    [Required]
    public string Name { get; set;}

    [Required]
    public List<Address> Addresses { get; set; }

    public int Rating { get; set; }
}
