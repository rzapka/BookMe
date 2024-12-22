using BookMe.Application.Service.Queries.SearchOffers;
using BookMe.Application.Service.Queries.SearchCities;
using BookMe.Application.Service.Queries.SearchServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("Search")]
public class SearchController : Controller
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("SearchOffers")]
    public async Task<IActionResult> SearchOffers(string term)
    {
        var offerNames = await _mediator.Send(new SearchOffersQuery { Term = term });
        return Json(offerNames);
    }

    [HttpGet("SearchCities")]
    public async Task<IActionResult> SearchCities(string term)
    {
        var cities = await _mediator.Send(new SearchCitiesQuery { Term = term });
        return Json(cities);
    }

    [HttpGet("Results")]
    public async Task<IActionResult> Results(string searchTerm, string city)
    {
        var services = await _mediator.Send(new SearchServicesByOfferAndCityQuery { SearchTerm = searchTerm, City = city });
        var model = new SearchResultsViewModel
        {
            SearchTerm = searchTerm,
            City = city,
            Services = services
        };
        return View(model);
    }
}
