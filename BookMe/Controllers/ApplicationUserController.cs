using AutoMapper;
using BookMe.Application.ApplicationUser.Dto;
using BookMe.Application.ApplicationUser.Commands.CreateApplicationUser;
using BookMe.Application.ApplicationUser.Commands.DeleteApplicationUser;
using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using BookMe.Application.ApplicationUser.Queries.GetApplicationUserById;
using BookMe.Application.ApplicationUser.Queries.GetApplicationUsers;
using BookMe.Application.Service.Queries.GetAllServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BookMe.Application.Exceptions;
using BookMe.Application.ApplicationUser;
using BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser;
using BookMe.Application.ApplicationUser.Commands.LoginApplicationUser;
using Microsoft.AspNetCore.Identity;
using BookMe.Domain.Entities;

namespace BookMe.Controllers
{
    [Route("Uzytkownicy")]
    public class ApplicationUserController : Controller
    {
        private readonly IUserContext _userContext;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApplicationUserController(IUserContext userContext, IMediator mediator, IMapper mapper,
           SignInManager<ApplicationUser> signInManager)
        {
            _userContext = userContext;
            _mediator = mediator;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet("Zarejestruj")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Zarejestruj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterApplicationUserCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(command);
        }

        [AllowAnonymous]
        [HttpGet("Zaloguj")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Zaloguj")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginApplicationUserCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }
            return View(command);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Utworz")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Utworz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreateApplicationUserCommand command)
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
                    foreach (var error in ex.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.ErrorMessage);
                    }
                }
            }
            return View("Create", command);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Lista")]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var query = new GetApplicationUsersQuery(searchTerm);
            var users = await _mediator.Send(query);

            var userDtos = _mapper.Map<List<ApplicationUserDto>>(users);
            ViewBag.SearchTerm = searchTerm;
            return View(userDtos);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Edytuj/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var query = new GetApplicationUserByIdQuery { Id = id };
            var user = await _mediator.Send(query);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View(_mapper.Map<UpdateApplicationUserCommand>(user));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Edytuj/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateApplicationUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            try
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            catch (UserEmailConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }

            ViewBag.Services = await _mediator.Send(new GetAllServicesQuery());
            return View("Edit", command);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Usun/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _mediator.Send(new GetApplicationUserByIdQuery { Id = id });
            if (user != null)
            {
                await _mediator.Send(new DeleteApplicationUserCommand(id));
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet("EdytujMojeKonto")]
        public async Task<IActionResult> EditMyAccount()
        {
            var currentUser = await _userContext.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound("Nie znaleziono zalogowanego użytkownika.");
            }

            var user = await _mediator.Send(new GetApplicationUserByIdQuery { Id = currentUser.Id });
            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<UpdateApplicationUserCommand>(user)); ;
        }

        [Authorize]
        [HttpPost("EdytujMojeKonto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyAccount(UpdateApplicationUserCommand command)
        {
            var currentUser = await _userContext.GetCurrentUserAsync();
            if (currentUser == null || command.Id != currentUser.Id)
            {
                return BadRequest("Niepoprawne dane użytkownika.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(command);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View("EditMyAccount", command);
        }

        [Authorize]
        [HttpPost("UsunMojeKonto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var currentUser = await _userContext.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return NotFound("Nie znaleziono zalogowanego użytkownika.");
            }

            var command = new DeleteApplicationUserCommand(currentUser.Id);
            await _mediator.Send(command);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("BrakUprawnien")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
