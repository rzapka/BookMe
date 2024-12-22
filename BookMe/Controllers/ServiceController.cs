using BookMe.Application.Service.Queries.GetServiceDetailsByEncodedName;
using BookMe.Application.Service.Queries.GetAllServices;
using BookMe.Application.Service.Queries.SearchServices;
using BookMe.Application.Service.Commands.CreateService;
using BookMe.Application.Service.Commands.UpdateService;
using BookMe.Application.Service.Commands.DeleteService;
using BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories;
using BookMe.Application.ApplicationUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookMe.Application.Service.Dto;
using BookMe.Application.Service.Queries.GetServiceById;
using AutoMapper;
using FluentValidation;
using BookMe.Application.Exceptions;
using BookMe.Application.ServiceImage.Queries.GetServiceImagesByServiceId;

namespace BookMe.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ServiceController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public ServiceController(IMediator mediator, IUserContext userContext, IMapper mapper)
        {
            _mediator = mediator;
            _userContext = userContext;
            _mapper = mapper;
        }

        [Route("{encodedName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceDetailsByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var isEmployee = await _userContext.IsEmployeeAsync();

            ViewData["IsEmployee"] = isEmployee;
            ViewData["ServiceImages"] = await _mediator.Send(new GetServiceImagesByServiceIdQuery { ServiceId = service.Id});
            

            return View(service);
        }

        [Route("/Serwisy/Lista")]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var services = string.IsNullOrWhiteSpace(searchTerm)
                ? await _mediator.Send(new GetAllServicesQuery())
                : await _mediator.Send(new SearchServicesByNameQuery { SearchTerm = searchTerm });

            ViewBag.SearchTerm = searchTerm;
            return View(services);
        }

        [Route("Serwis/Tworzenie")]
        public async Task<IActionResult> Create()
        {
            ViewBag.ServiceCategories = await _mediator.Send(new GetAllServiceCategoriesQuery());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Serwis/Tworzenie")]
        public async Task<IActionResult> Create(CreateServiceCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.ServiceCategories = await _mediator.Send(new GetAllServiceCategoriesQuery());
            return View(command);
        }


        [Route("Serwis/{encodedName}/Edycja")]
        public async Task<IActionResult> Edit(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceDetailsByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }
            var command = _mapper.Map<UpdateServiceCommand>(service);
            ViewBag.ServiceCategories = await _mediator.Send(new GetAllServiceCategoriesQuery());
            return View(command);
        }

        [HttpPost("Serwis/{encodedName}/Edycja")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateServiceCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction(nameof(Index));
                }
                catch (ServiceNameConflictException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.ServiceCategories = await _mediator.Send(new GetAllServiceCategoriesQuery());
            return View(command);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string encodedName)
        {
            await _mediator.Send(new DeleteServiceCommand { EncodedName = encodedName });
            return RedirectToAction(nameof(Index));
        }
    }
}
