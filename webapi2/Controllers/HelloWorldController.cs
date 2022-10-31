using Microsoft.AspNetCore.Mvc;
using proyectoef;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloWorldController:  ControllerBase
{
    private readonly ILogger<HelloWorldController> _logger;
    IHelloWorldService helloWorldService;

    TareasContext dbcontext;
    public HelloWorldController(ILogger<HelloWorldController> logger, IHelloWorldService helloWorld, TareasContext db)
    {
        _logger = logger;
        helloWorldService = helloWorld;
        dbcontext =db;
    }

    [HttpGet]
    [Route("createdb")]
    public IActionResult CreateDataBase()
    {
        dbcontext.Database.EnsureCreated();
        return Ok();
    }

    [HttpGet]    
    public IActionResult Get()
    {
        _logger.LogInformation("Retornando 1");
        return Ok(helloWorldService.GetHelloWorld());
    }
}