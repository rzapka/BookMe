using BookMe.Application.Employee.Commands.CreateEmployee;
using BookMe.Application.Employee.Commands.DeleteEmployee;
using BookMe.Application.Employee.Queries.GetEmployeesByServiceEncodedName;
using BookMe.Application.Employee.Queries.SearchEmployees;
using BookMe.Application.Service.Queries.GetServiceByEncodedName;
using BookMe.Application.Service.Queries.GetAllServices;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Application.Employee.Queries.GetEmployeeById;
using BookMe.Application.Exceptions;
using BookMe.Application.Service.Queries.GetServiceById;

namespace BookMe.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;

        public EmployeeController(IMediator mediator, UserManager<Domain.Entities.ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        // Akcja do zarządzania pracownikami z przypisanym serwisem
        [HttpGet("Pracownicy/{encodedName}/Lista")]
        public async Task<IActionResult> Index(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = service.Name;
            ViewBag.ServiceEncodedName = encodedName;

            var employees = await _mediator.Send(new GetEmployeesByServiceEncodedNameQuery { ServiceEncodedName = encodedName });
            return View(employees);
        }

        // Nowa akcja do tworzenia pracownika bez przypisanego serwisu
        [HttpGet("Pracownicy/Utworz")]
        public async Task<IActionResult> CreateWithoutService()
        {
            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View();
        }

        [HttpPost("Pracownicy/UtworzBezSerwisu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithoutServicePost(CreateEmployeeCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("AdminIndex");
                }
                catch (ValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }

            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View("CreateWithoutService", command);
        }

        [HttpPost("Pracownicy/{id}/Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string encodedName)
        {
            try
            {
                await _mediator.Send(new DeleteEmployeeCommand { Id = id });
                if (encodedName == null)
                {
                    return RedirectToAction("AdminIndex");
                }
                return RedirectToAction("Index", new { encodedName });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Pracownicy/{encodedName}/Utworz")]
        public async Task<IActionResult> Create(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }
            ViewBag.ServiceEncodedName = encodedName;
            ViewBag.ServiceId = service.Id;
            return View();
        }

        [HttpPost("Pracownicy/{encodedName}/Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string encodedName, CreateEmployeeCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", new { encodedName });
                }
                catch (ValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
                catch (UserEmailConflictException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            ViewBag.ServiceEncodedName = encodedName;
            return View(command);
        }

        [HttpGet("Pracownicy/ListaWszystkich")]
        public async Task<IActionResult> AdminIndex(string searchTerm = "")
        {
            var employees = await _mediator.Send(new SearchEmployeesQuery { SearchTerm = searchTerm });
            ViewBag.SearchTerm = searchTerm;
            return View(employees);
        }

        [HttpGet("Pracownicy/{id}/Edycja")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery { Id = id });
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View(employee);
        }

        [HttpPost("Pracownicy/Edycja")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateEmployeeCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
                return View("Edit", command);
            }

            try
            {
                await _mediator.Send(command);
                var service = await _mediator.Send(new GetServiceByIdQuery{ Id = command.ServiceId });
                return RedirectToAction("Index", new { encodedName = service.EncodedName });
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }
            
            catch (UserEmailConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View("Edit", command);
        }

    }
}
