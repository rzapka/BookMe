using BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories;
using BookMe.Application.Service.Queries.GetRecommendedServices;
using BookMe.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var serviceCategories = await _mediator.Send(new GetAllServiceCategoriesQuery());
        var recommendedServices = await _mediator.Send(new GetRecommendedServicesQuery());

        var viewModel = new HomeViewModel
        {
            ServiceCategories = serviceCategories,
            RecommendedServices = recommendedServices
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
