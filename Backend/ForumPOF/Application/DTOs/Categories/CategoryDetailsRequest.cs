﻿namespace Application.DTOs.Categories;

public class CategoryDetailsRequest
{
    public Ulid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
}
