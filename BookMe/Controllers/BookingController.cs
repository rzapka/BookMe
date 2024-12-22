using AutoMapper;
using BookMe.Application.ApplicationUser;
using BookMe.Application.Booking.Commands.CreateBooking;
using BookMe.Application.Booking.Commands.DeleteBooking;
using BookMe.Application.Booking.Commands.UpdateBooking;
using BookMe.Application.Booking.Dto;
using BookMe.Application.Booking.Queries.GetBookingById;
using BookMe.Application.Booking.Queries.GetBookingCreationData;
using BookMe.Application.Booking.Queries.GetBookingDetails;
using BookMe.Application.Booking.Queries.GetBookingsByServiceEncodedName;
using BookMe.Application.Booking.Queries.ListBookings;
using BookMe.Application.Employee.Queries.GetEmployeesAsDictionary;
using BookMe.Application.Notification.Commands.CreateNotification;
using BookMe.Application.Offer.Queries.GetOfferByEncodedName;
using BookMe.Application.Offer.Queries.GetOfferById;
using BookMe.Application.Opinion.Queries.GetServiceByOpinionId;
using BookMe.Application.Service.Queries.GetServiceByEncodedName;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class BookingController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public BookingController(
        IMediator mediator,
        IUserContext userContext,
        IMapper mapper)
    {
        _mediator = mediator;
        _userContext = userContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("{serviceEncodedName}/{offerEncodedName}/Wizyta")]
    public async Task<IActionResult> Create(string serviceEncodedName, string offerEncodedName)
    {
        var query = new GetBookingCreationDataQuery { ServiceEncodedName = serviceEncodedName, OfferEncodedName = offerEncodedName };
        var data = await _mediator.Send(query);

        if (data == null || data.Offer == null)
        {
            return NotFound("Nie znaleziono oferty");
        }

        ViewBag.Offer = data.Offer;
        ViewBag.Employees = data.Employees;
        ViewBag.ServiceEncodedName = serviceEncodedName;

        return View();
    }

    [HttpPost]
    [Route("{serviceEncodedName}/{offerEncodedName}/Wizyta")]
    public async Task<IActionResult> Create(string serviceEncodedName, string offerEncodedName, CreateBookingCommand command)
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "ApplicationUser");
        }

        command.UserId = currentUser.Id;

        var offer = await _mediator.Send(new GetOfferByEncodedNameQuery { ServiceEncodedName = serviceEncodedName, OfferEncodedName = offerEncodedName });
        if (offer == null)
        {
            return NotFound("Nie znaleziono oferty");
        }

        command.OfferId = offer.Id;
        command.Offer = offer;

        try
        {
            await _mediator.Send(command);
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }

            var data = await _mediator.Send(new GetBookingCreationDataQuery { ServiceEncodedName = serviceEncodedName, OfferEncodedName = offerEncodedName });
            ViewBag.Offer = data.Offer;
            ViewBag.Employees = data.Employees;
            ViewBag.ServiceEncodedName = serviceEncodedName;
            ViewBag.OfferEncodedName = offerEncodedName;
            return View(command);
        }

        await CreateNotificationAsync(command.EmployeeId, $"Nowa wizyta od {currentUser.FirstName} {currentUser.LastName} zaplanowana na {command.StartTime}");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route("Edytuj/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "ApplicationUser");
        }

        var isEmployee = await _userContext.IsEmployeeAsync();
        if (isEmployee)
        {
            return Forbid();
        }

        var booking = await _mediator.Send(new GetBookingByIdQuery { Id = id });
        if (booking == null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateBookingCommand>(booking);
        var employees = await _mediator.Send(new GetEmployeesAsDictionaryQuery { ServiceEncodedName =  command.ServiceEncodedName});

        ViewBag.Employees = employees;

        return View(command);
    }

    [HttpPost]
    [Route("Edytuj/{id}")]
    public async Task<IActionResult> Edit(int id, UpdateBookingCommand command, string serviceEncodedName)
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "ApplicationUser");
        }

        var isEmployee = await _userContext.IsEmployeeAsync();
        if (isEmployee)
        {
            return Forbid();
        }

        if (id != command.Id)
        {
            return BadRequest();
        }

        var offer = await _mediator.Send(new GetOfferByEncodedNameQuery {ServiceEncodedName = serviceEncodedName, OfferEncodedName = command.OfferEncodedName });
        if (offer == null)
        {
            return NotFound("Nie znaleziono oferty");
        }

        command.OfferId = offer.Id;
        command.Offer = offer;
        command.SetEndTime();

        try
        {
            await _mediator.Send(command);
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }

            var employees = await _mediator.Send(new GetEmployeesAsDictionaryQuery { ServiceEncodedName = serviceEncodedName });
            ViewBag.Employees = employees;

            return View(command);
        }

        await CreateNotificationAsync(command.EmployeeId, $"Wizyta z {currentUser.FirstName} {currentUser.LastName} została zaktualizowana na {command.StartTime}");

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Usun/{id}")]
    public async Task<IActionResult> Delete(int id, int employeeId)
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "ApplicationUser");
        }

        var isEmployee = await _userContext.IsEmployeeAsync();
        if (isEmployee)
        {
            return Forbid();
        }

        var command = new DeleteBookingCommand { Id = id };
        try
        {
            await _mediator.Send(command);
            await CreateNotificationAsync(employeeId, $"Wizyta użytkownika {currentUser.FirstName} {currentUser.LastName} została anulowana");

            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [Route("Wizyty")]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null || !User.Identity.IsAuthenticated || User.IsInRole("Admin"))
        {
            return Unauthorized();
        }

        var isEmployee = await _userContext.IsEmployeeAsync();
        ViewBag.IsEmployee = isEmployee;

        var bookings = await _mediator.Send(new ListBookingsQuery
        {
            UserId = currentUser.Id,
            IsEmployee = isEmployee
        });

        var detailedBookings = new List<BookingDto>();
        foreach (var booking in bookings)
        {
            var detailedBooking = await _mediator.Send(new GetBookingDetailsQuery { BookingId = booking.Id });
            detailedBookings.Add(detailedBooking);
        }

        return View(detailedBookings);
    }

    private async Task CreateNotificationAsync(int? employeeId, string message)
    {
        if (employeeId.HasValue)
        {
            var command = new CreateNotificationCommand
            {
                EmployeeId = employeeId.Value,
                Message = message
            };

            await _mediator.Send(command);
        }
    }

    [Authorize(Roles = "ADMIN")]
    [Route("Admin/Wizyty/{encodedName}")]
    public async Task<IActionResult> AdminIndex(string encodedName, string searchTerm = null)
    {
        var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
        if (service == null)
        {
            return NotFound("Nie znaleziono serwisu");
        }

        ViewBag.ServiceName = service.Name;
        ViewBag.ServiceEncodedName = encodedName;
        ViewBag.SearchTerm = searchTerm;

        var bookings = await _mediator.Send(new GetBookingsByServiceEncodedNameQuery
        {
            EncodedName = encodedName,
            SearchTerm = searchTerm
        });

        return View(bookings);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("Admin/Wizyty/Edytuj/{id}")]
    public async Task<IActionResult> AdminEdit(int id)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery { Id = id });
        if (booking == null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateBookingCommand>(booking);

        var employees = await _mediator.Send(new GetEmployeesAsDictionaryQuery { ServiceEncodedName = booking.Service.EncodedName });
        ViewBag.Employees = employees;

        return View(command);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost("Admin/Wizyty/Edytuj/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminEdit(int id, UpdateBookingCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var offer = await _mediator.Send(new GetOfferByIdQuery { Id = command.OfferId });
        if (offer == null)
        {
            return NotFound("Nie znaleziono oferty");
        }

        command.Offer = offer;
        command.SetEndTime();

        try
        {
            await _mediator.Send(command);
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }

            var employees = await _mediator.Send(new GetEmployeesAsDictionaryQuery { ServiceEncodedName = command.ServiceEncodedName });
            ViewBag.Employees = employees;

            return View(command);
        }

        await CreateNotificationAsync(command.EmployeeId,
            $"Wizyta użytkownika {command.UserFullName} została zaktualizowana na {command.StartTime}.");

        return RedirectToAction("AdminIndex", new { encodedName = offer.Service.EncodedName });
    }


    [Authorize(Roles = "ADMIN")]
    [HttpPost("Admin/Usun/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AdminDelete(int id)
    {
        var booking = await _mediator.Send(new GetBookingByIdQuery { Id = id });

        if (booking == null)
        {
            return NotFound();
        }


        try
        {
            await _mediator.Send(new DeleteBookingCommand { Id = id });
            await CreateNotificationAsync(booking.EmployeeId,
                $"Wizyta z {booking.StartTime} użytkownika {booking.User.FirstName} {booking.User.LastName} została usunięta.");

            return RedirectToAction("AdminIndex", new { booking.Service.EncodedName });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}
