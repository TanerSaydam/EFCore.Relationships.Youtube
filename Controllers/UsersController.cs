using EFCore.Relationships.Youtube.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Relationships.Youtube.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public sealed class UsersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public IActionResult GetAll()
    {
        var users =
            context
            .Users
            .Include(p => p.UserInformation)
            .Select(p => new
            {
                Id = p.Id,
                FullName = p.FullName
            })
            .AsQueryable();

        return Ok(users);
    }
}
