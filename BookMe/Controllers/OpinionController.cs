using AutoMapper;
using BookMe.Application.ApplicationUser;
using BookMe.Application.Booking.Queries.GetBookingById;
using BookMe.Application.Employee.Dto;
using BookMe.Application.Employee.Queries.GetEmployeesByServiceId;
using BookMe.Application.Offer.Dto;
using BookMe.Application.Offer.Queries.GetOffersByServiceId;
using BookMe.Application.Opinion.Commands.CreateOpinion;
using BookMe.Application.Opinion.Commands.DeleteOpinion;
using BookMe.Application.Opinion.Commands.UpdateOpinion;
using BookMe.Application.Opinion.Dto;
using BookMe.Application.Opinion.Queries.GetOpinionById;
using BookMe.Application.Opinion.Queries.GetOpinionsByServiceEncodedName;
using BookMe.Application.Opinion.Queries.GetServiceByOpinionId;
using BookMe.Application.Service.Queries.GetServiceDetailsByEncodedName;
using BookMe.Application.Sieve;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sieve.Models;
using BookMe.Application.Pagination;
using BookMe.Application.Booking.Dto;

namespace BookMe.Controllers
{
    [Route("Opinie")]
    public class OpinionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;
        private readonly IOptions<SieveOptions> _options;
        private readonly IMapper _mapper;

        public OpinionController(IMediator mediator, IUserContext userContext,
            IOptions<SieveOptions> options, IMapper mapper)
        {
            _mediator = mediator;
            _userContext = userContext;
            _options = options;
            _mapper = mapper;
        }

        [HttpPost("PobierzOpinie/{encodedName}")]
        public async Task<IActionResult> GetOpinionsWithSieve(string encodedName, [FromBody] SieveModel query)
        {
            query.Sorts = "-CreatedAt";

            var opinions = await _mediator.Send(new GetOpinionsByServiceEncodedNameQuery { EncodedName = encodedName });
            var opinionsQuery = opinions.AsQueryable();

            var sieveProcessor = new ApplicationSieveProcessor(_options);
            opinionsQuery = sieveProcessor.Apply(query, opinionsQuery, applyPagination: false);

            var totalCount = opinionsQuery.Count();

            var pagedOpinions = opinionsQuery
                .Skip((query.Page.Value - 1) * query.PageSize.Value)
                .Take(query.PageSize.Value)
                .ToList();

            var result = new PagedResult<OpinionDto>(pagedOpinions, totalCount, query.PageSize.Value, query.Page.Value);

            return Ok(result);
        }

        [HttpGet("Dodaj/{bookingId?}")]
        public async Task<IActionResult> Create(int? bookingId)
        {
            if (!bookingId.HasValue)
            {
                return BadRequest("BookingId jest wymagane.");
            }

            var booking = await _mediator.Send(new GetBookingByIdQuery { Id = bookingId.Value });
            
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDto = _mapper.Map<BookingDto>(booking);
            if (bookingDto.OpinionId != null)
            {
                return RedirectToAction("Edit", new { id = bookingDto.OpinionId });
            }

            var command = new CreateOpinionCommand
            {
                ServiceId = bookingDto.ServiceId,
                OfferId = bookingDto.OfferId,
                EmployeeId = bookingDto.EmployeeId,
                UserId = bookingDto.UserId,
                BookingId = bookingDto.Id,
                ServiceEncodedName = bookingDto.ServiceEncodedName,
                Rating = 1
            };

            ViewBag.ServiceName = bookingDto.ServiceName;
            ViewBag.OfferName = bookingDto.OfferName;
            ViewBag.EmployeeFullName = bookingDto.EmployeeFullName;
            ViewBag.OfferPrice = bookingDto.OfferPrice;
            ViewBag.StartTime = bookingDto.StartTime;
            ViewBag.OfferDuration = bookingDto.OfferDuration;

            return View(command);
        }

        [HttpPost("Dodaj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreateOpinionCommand command)
        {
            var currentUser = await _userContext.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "ApplicationUser");
            }

            command.UserId = currentUser.Id;

            await _mediator.Send(command);

            return RedirectToAction("Details", "Service", new { encodedName = command.ServiceEncodedName });
        }

        [HttpGet("Edytuj/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var opinion = await _mediator.Send(new GetOpinionByIdQuery { Id = id });
            if (opinion == null)
            {
                return NotFound();
            }

            var command = _mapper.Map<UpdateOpinionCommand>(opinion);

            return View(command);
        }

        [HttpPost("Edytuj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UpdateOpinionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", command);
            }

            await _mediator.Send(command);
            return RedirectToAction("Details", "Service", new { encodedName = command.ServiceEncodedName });
        }

        [HttpPost("Usun/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOpinion(int id)
        {
            await _mediator.Send(new DeleteOpinionCommand { Id = id });
            return RedirectToAction("Index", "Booking");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Opinia/{id}/Edycja")]
        public async Task<IActionResult> AdminEdit(int id)
        {
            var opinion = await _mediator.Send(new GetOpinionByIdQuery { Id = id });
            if (opinion == null)
            {
                return NotFound();
            }

            var employees = await _mediator.Send(new GetEmployeesByServiceIdQuery { ServiceId = opinion.ServiceId }) ?? new List<EmployeeDto>();
            var offers = await _mediator.Send(new GetOffersByServiceIdQuery { ServiceId = opinion.ServiceId }) ?? new List<OfferDto>();

            var command = _mapper.Map<UpdateOpinionCommand>(opinion);
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            ViewBag.Offers = new SelectList(offers, "Id", "Name");

            return View(command);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("EdycjaAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEditPost(UpdateOpinionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminEdit", command);
            }

            await _mediator.Send(command);
            return RedirectToAction(nameof(Index), new { encodedName = command.ServiceEncodedName });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("UsunAdmin/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDelete(int id)
        {
            var serviceDto = await _mediator.Send(new GetServiceByOpinionIdQuery { Id = id });
            await _mediator.Send(new DeleteOpinionCommand { Id = id });
            return RedirectToAction(nameof(Index), new { encodedName = serviceDto.EncodedName });
        }

        [Route("Opinie/{encodedName}/Lista")]
        public async Task<IActionResult> Index(string encodedName)
        {
            var opinions = await _mediator.Send(new GetOpinionsByServiceEncodedNameQuery { EncodedName = encodedName });
            ViewBag.Service = await _mediator.Send(new GetServiceDetailsByEncodedNameQuery(encodedName));
            return View(opinions);
        }
    }
}
