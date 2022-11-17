﻿namespace ProductApi.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Discount { get; set; }
}
