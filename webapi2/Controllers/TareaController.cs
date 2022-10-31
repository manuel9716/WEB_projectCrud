using System.Net;
using Microsoft.AspNetCore.Mvc;
using proyectoef.Models;
using webapi2.Services;

namespace webapi2.Controllers;

[Route("api/[controller]")]

public class TareaController: ControllerBase
{
    ITareasService tareasService;

    public TareaController(ITareasService service)
    {
        tareasService = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(tareasService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Tarea tarea)
    {
        tareasService.Save(tarea);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Tarea tarea)
    {
        tareasService.Update(id, tarea);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete (Guid id)
    {
        tareasService.Delete(id);
        return Ok();
    }
}