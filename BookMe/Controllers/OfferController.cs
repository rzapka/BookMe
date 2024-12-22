using AutoMapper;
using BookMe.Application.Offer.Commands.CreateOffer;
using BookMe.Application.Offer.Commands.DeleteOffer;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.Offer.Dto;
using BookMe.Application.Offer.Queries.GetOfferById;
using BookMe.Application.Offer.Queries.GetOffersByServiceEncodedName;
using BookMe.Application.Service.Queries.GetServiceByEncodedName;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMe.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class OfferController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OfferController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("Oferty/{encodedName}/Lista")]
        public async Task<IActionResult> Index(string encodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(encodedName));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceName = service.Name;
            ViewBag.ServiceId = service.Id;
            ViewBag.ServiceEncodedName = encodedName;

            var offers = await _mediator.Send(new GetOffersByServiceEncodedNameQuery { ServiceEncodedName = encodedName });
            return View(offers);
        }

        [HttpGet("Oferty/{serviceEncodedName}/Utworz")]
        public async Task<IActionResult> Create(string serviceEncodedName)
        {
            var service = await _mediator.Send(new GetServiceByEncodedNameQuery(serviceEncodedName));
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.ServiceEncodedName = serviceEncodedName;
            ViewBag.ServiceId = service.Id;
            return View();
        }

        [HttpPost("Oferty/{serviceEncodedName}/Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string serviceEncodedName, CreateOfferCommand command)
        {


            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction("Index", new { encodedName = serviceEncodedName });
            }

            ViewBag.ServiceEncodedName = serviceEncodedName;
            return View(command);
        }

        [HttpGet("Oferty/{id}/Edycja")]
        public async Task<IActionResult> Edit(int id)
        {
            var offer = await _mediator.Send(new GetOfferByIdQuery { Id = id });
            if (offer == null)
            {
                return NotFound();
            }
            var command = _mapper.Map<UpdateOfferCommand>(offer);
            ViewBag.ServiceEncodedName = command.ServiceEncodedName; 
            return View(command);
        }

        [HttpPost("Oferty/Edycja")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateOfferCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", new { encodedName = command.ServiceEncodedName });
                }
                catch (KeyNotFoundException)
                {
                    ModelState.AddModelError(string.Empty, "Offer not found.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving changes. Please try again.");
                }
            }

            ViewBag.EncodedName = command.ServiceEncodedName;
            return View("Edit", command); 
        }

        [HttpPost("Oferty/{id}/Usun")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string encodedName)
        {
            try
            {
                await _mediator.Send(new DeleteOfferCommand { Id = id });
                return RedirectToAction("Index", new { encodedName });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
