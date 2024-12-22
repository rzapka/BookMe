using BookMe.Application.Helpers;
using BookMe.Application.OpeningHours.Commands.CreateOpeningHour;
using BookMe.Application.OpeningHours.Commands.DeleteOpeningHour;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.OpeningHours.Queries.GetOpeningHourById;
using BookMe.Application.OpeningHours.Queries.GetOpeningHoursByServicedEncodedName;
using BookMe.Application.OpeningHours.Queries.GetTakenDaysOfWeekByServiceId;
using BookMe.Application.Service.Queries.GetServiceByEncodedName;
using BookMe.Application.Service.Queries.GetServiceDetailsByEncodedName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMe.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class OpeningHoursController : Controller
    {
        private readonly IMediator _mediator;

        public OpeningHoursController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("GodzinyOtwarcia/{encodedName}/Lista")]
        public async Task<IActionResult> Index(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = service.Name;
            ViewBag.EncodedName = service.EncodedName;
     
            var openingHours = await _mediator.Send(new GetOpeningHoursByServiceEncodedNameQuery { EncodedName = encodedName });
            return View(openingHours); 
        }

        [HttpGet("GodzinyOtwarcia/{encodedName}/Utworz")]
        public async Task<IActionResult> Create(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            var existingDays = await _mediator.Send(new GetTakenDaysOfWeekByServiceIdQuery { ServiceId = service.Id });
            var allDaysOfWeek = WeekDaysProvider.GetPolishDaysOfWeek();
            var availableDays = allDaysOfWeek.Except(existingDays).ToList();

            if (!availableDays.Any())
            {
                TempData["ErrorMessage"] = "Wszystkie dni mają już ustawione godziny otwarcia. Nie można dodać więcej.";
                return RedirectToAction("Index", new { encodedName });
            }

            ViewBag.DaysOfWeek = availableDays;
            ViewBag.EncodedName = encodedName;
            ViewBag.ServiceId = service.Id;

            return View();
        }


        [HttpPost("GodzinyOtwarcia/{encodedName}/Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string encodedName, CreateOpeningHourCommand command)
        {
            if (!ModelState.IsValid)
            {
                // Populate available days again if there are validation errors
                var existingDays = await _mediator.Send(new GetTakenDaysOfWeekByServiceIdQuery { ServiceId = command.ServiceId });
                var allDaysOfWeek = WeekDaysProvider.GetPolishDaysOfWeek();
                ViewBag.DaysOfWeek = allDaysOfWeek.Except(existingDays).ToList();
                ViewBag.EncodedName = encodedName;

                return View(command);
            }

            // No need for command.ValidationResult, since ModelState already covers validation.
            await _mediator.Send(command);

            // Redirect to the index after successful creation
            return RedirectToAction("Index", new { encodedName });
        }

        [HttpGet("GodzinyOtwarcia/{id}/Edycja")]
        public async Task<IActionResult> Edit(int id)
        {
            var command = await _mediator.Send(new GetOpeningHourByIdQuery { Id = id });
            if (command == null)
            {
                return NotFound();
            }

            var existingDays = await _mediator.Send(new GetTakenDaysOfWeekByServiceIdQuery { ServiceId = command.ServiceId });

            var allDaysOfWeek = WeekDaysProvider.GetPolishDaysOfWeek();
            var availableDays = allDaysOfWeek.Except(existingDays.Where(day => day != command.DayOfWeek)).ToList();

            ViewBag.DaysOfWeek = availableDays;
            ViewBag.ServiceEncodedName = command.ServiceEncodedName;

            return View(command); 
        }


        [HttpPost("OpeningHour/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateOpeningHourCommand command)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate days of the week on validation error
                var existingDays = await _mediator.Send(new GetTakenDaysOfWeekByServiceIdQuery { ServiceId = command.ServiceId });
                var allDaysOfWeek = WeekDaysProvider.GetPolishDaysOfWeek();
                ViewBag.DaysOfWeek = allDaysOfWeek.Except(existingDays.Where(day => day != command.DayOfWeek)).ToList();

                return View("Edit", command);
            }

            await _mediator.Send(command);

            return RedirectToAction("Index", new { encodedName = command.ServiceEncodedName });
        }


        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string encodedName)
        {
            try
            {
                await _mediator.Send(new DeleteOpeningHourCommand { Id = id });
                return RedirectToAction("Index", new { encodedName });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
