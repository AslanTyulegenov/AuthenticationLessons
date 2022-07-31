using Calabonga.DemoClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Swagger.Controllers;

[Route("api")]
public class ApiController : ControllerBase
{
    private readonly List<Person> _people = People.GetPeople();

    [HttpGet("[action]")]
    [Authorize]
    public IActionResult GetAll()
    {
        return Ok(_people);
    }

    [HttpGet("[action]/{id:int}")]
    public IActionResult GetById(int id)
    {
        var result = _people.FirstOrDefault(p => p.Id == id);

        return Ok(result);
    }
}
