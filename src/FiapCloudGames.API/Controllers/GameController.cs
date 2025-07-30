using FiapCloudGames.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class GameController : ControllerBase
{

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Post()
    {
        return Ok();
    }
}