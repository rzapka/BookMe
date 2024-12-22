using BookMe.Application.ServiceCategory.Commands.CreateServiceCategory;
using BookMe.Application.ServiceCategory.Commands.DeleteServiceCategory;
using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;
using BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories;
using BookMe.Application.ServiceCategory.Queries.GetServiceCategoryByEncodedName;
using BookMe.Application.ServiceCategory.Queries.GetServiceCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMe.Controllers
{
    public class ServiceCategoryController : Controller
    {
        private readonly IMediator _mediator;

        public ServiceCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("Kategorie/Lista")]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var categories = await _mediator.Send(new GetAllServiceCategoriesQuery { SearchTerm = searchTerm });
            ViewBag.SearchTerm = searchTerm;
            return View(categories);
        }

        [Route("Kategorie/{encodedName}")]
        public async Task<IActionResult> Details(string encodedName, string searchTerm = "")
        {
            var category = await _mediator.Send(new GetServiceCategoryByEncodedNameQuery(encodedName, searchTerm));
            if (category == null)
            {
                return NotFound();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(category);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("Kategorie/Administracja")]
        public async Task<IActionResult> ServiceCategories()
        {
            var categories = await _mediator.Send(new GetAllServiceCategoriesQuery());
            return View(categories);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("Kategorie/Utworz")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Kategorie/Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceCategoryCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction("Index");
            }
            return View(command);
        }

        [Authorize(Roles = "ADMIN")]
        [Route("Kategorie/{id}/Edycja")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _mediator.Send(new GetServiceCategoryByIdQuery { Id = id });
            if (category == null)
            {
                return NotFound();
            }
            return View(category); 
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Kategorie/Edycja")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateServiceCategoryCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction("Index");
            }
            return View("Edit", command);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Kategorie/{id}/Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteServiceCategoryCommand { Id = id });
            return RedirectToAction("Index");
        }
    }
}
