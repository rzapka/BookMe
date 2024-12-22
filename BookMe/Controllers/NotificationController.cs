using BookMe.Application.ApplicationUser;
using BookMe.Application.Employee.Queries.GetEmployeeByUserId;
using BookMe.Application.Notification.Commands.MarkNotificationAsRead;
using BookMe.Application.Notification.Dto;
using BookMe.Application.Notification.Queries.GetNotifcationsForEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Authorize]
[Route("[controller]")]
public class NotificationController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public NotificationController(IMediator mediator, IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userContext.GetCurrentUserAsync();
        if (currentUser == null)
        {
            return RedirectToAction("Login", "ApplicationUser");
        }

        var employee = await _mediator.Send(new GetEmployeeByUserIdQuery { UserId = currentUser.Id });
        if (employee == null)
        {
            return Unauthorized();
        }

      
        var notifications = await _mediator.Send(new GetNotificationsForEmployeeQuery { EmployeeId = employee.Id });
        return View(notifications);
    }

    [HttpPost]
    [Route("MarkAsRead/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _mediator.Send(new MarkNotificationAsReadCommand { NotificationId = id });
            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
