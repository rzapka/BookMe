using BookMe.Application.Service.Queries.GetServiceByEncodedName;
using BookMe.Application.ServiceImage.Commands.CreateServiceImage;
using BookMe.Application.ServiceImage.Commands.DeleteServiceImage;
using BookMe.Application.ServiceImage.Commands.UpdateServiceImage;
using BookMe.Application.ServiceImage.Queries.GetServiceImageById;
using BookMe.Application.ServiceImage.Queries.GetServiceImagesByEncodedName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMe.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ServiceImageController : Controller
    {
        private readonly IMediator _mediator;

        public ServiceImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("ZdjeciaSerwisu/{encodedName}/Lista")]
        public async Task<IActionResult> Index(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = service.Name;
            ViewBag.EncodedName = service.EncodedName;
            ViewBag.ServiceId = service.Id;

            var serviceImages = await _mediator.Send(new GetServiceImagesByEncodedNameQuery { EncodedName = encodedName });
            return View(serviceImages);
        }

        [HttpGet("ZdjeciaSerwisu/{encodedName}/Utworz")]
        public async Task<IActionResult> Create(string encodedName)
        {
            ViewBag.EncodedName = encodedName;

            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            ViewBag.ServiceId = service.Id;
            return View(new CreateServiceImageCommand());
        }

        [HttpPost("ZdjeciaSerwisu/{encodedName}/Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string encodedName, CreateServiceImageCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EncodedName = encodedName;
                ViewBag.ServiceId = command.ServiceId;
                return View(command);
            }

            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            command.ServiceId = service.Id;
            await _mediator.Send(command);

            return RedirectToAction("Index", new { encodedName });
        }

        [HttpGet("ZdjeciaSerwisu/{encodedName}/Edycja/{id}")]
        public async Task<IActionResult> Edit(string encodedName, int id)
        {
            var serviceImage = await _mediator.Send(new GetServiceImageByIdQuery { Id = id });
            if (serviceImage == null)
            {
                return NotFound();
            }

            var command = new UpdateServiceImageCommand
            {
                Id = serviceImage.Id,
                Url = serviceImage.Url
            };

            ViewBag.EncodedName = encodedName;
            return View(command);
        }

        [HttpPost("ZdjeciaSerwisu/{encodedName}/Edycja/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string encodedName, UpdateServiceImageCommand command)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EncodedName = encodedName;
                return View(command);
            }

            await _mediator.Send(command);

            return RedirectToAction("Index", new { encodedName });
        }

        [HttpPost("ZdjeciaSerwisu/{encodedName}/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string encodedName, int id)
        {
            try
            {
                await _mediator.Send(new DeleteServiceImageCommand { Id = id });
                return RedirectToAction("Index", new { encodedName });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
