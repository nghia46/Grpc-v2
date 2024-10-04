using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/api")]
public class BaseController : ControllerBase
{
    [HttpGet("hi")]
    public IActionResult Get()
    {
        return Ok("Service B");
    }
}