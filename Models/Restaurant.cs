﻿using System.ComponentModel.DataAnnotations;
using foodies_api.Models;

namespace foodies_api;

public class Restaurant
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Branch> Branches { get; set; } = new List<Branch>();
    public int Rating { get; set; }
}
