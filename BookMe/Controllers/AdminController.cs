using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "ADMIN")]
public class AdminController : Controller
{

    [HttpGet("/Panel/Admina")]
    public IActionResult Index()
    {
        var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        return View();
    }


}
