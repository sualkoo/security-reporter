using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{

    [HttpPost]
    public IActionResult Post()
    {
        return Ok();
    }
}
