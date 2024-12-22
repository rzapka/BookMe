using System.Collections.Generic;
using BookMe.Domain.Entities;

public class SearchResultsViewModel
{
    public string SearchTerm { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public List<Service> Services { get; set; } = new();
}