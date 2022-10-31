using System.Reflection;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Mvc;//para usar FormServices
using Microsoft.EntityFrameworkCore;//para usar UseInMemoryDatabase
using proyectoef;
using proyectoef.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<TareasContext>(p => p.UseInMemoryDatabase("TareasDB"));//solo sirve para la sb en memoria
//Coneccion para SQLServer
builder.Services.AddSqlServer<TareasContext>(builder.Configuration.GetConnectionString("cnTareas"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async ([FromServices] TareasContext dbContext) => 
{
    dbContext.Database.EnsureCreated();//asegurarnos que se cree la base de datos
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

});

//mapeamos la ruta del endpoint luego colocamos async y decimos que desde la configuracion de los servicios vamos a traernos el tareasContext
//Enpoint GET para tareas
app.MapGet("/api/tareas", async ([FromServices] TareasContext dbContext) => 
{
    //filtro de mostrar solo los que tienen prioridad baja y tambien los datos que tiene categoria
    //return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria).Where(p=> p.PrioridadTarea == proyectoef.Models.Prioridad.Baja));
    return Results.Ok(dbContext.Tareas.Include(p=> p.Categoria));
});

//Endpoint para categorias
app.MapGet("/api/categorias", async ([FromServices] TareasContext dbContext) => 
{
    return Results.Ok(dbContext.Categorias);
});

//Enpoint Post para tareas
app.MapPost("/api/tareas", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea) => 
{
    tarea.TareaId = Guid.NewGuid();
    tarea.FechaCreacion = DateTime.Now;
    await dbContext.AddAsync(tarea);
    //await dbContext.Tareas.AddAsync(tareas);

    await dbContext.SaveChangesAsync();

    return Results.Ok();
});

//Enpoint Actualizar para tareas
app.MapPut("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromBody] Tarea tarea,[FromRoute] Guid id)=>
{
    //por defecto busca por la que este configurada como key
 var tareaActual = dbContext.Tareas.Find(id);

    //Con este ciclo se asigna la actualizacion a todos los campos de la id que indiquemos
    if(tareaActual!=null)
    {
        tareaActual.CategoriaId = tarea.CategoriaId;
        tareaActual.Titulo = tarea.Titulo;
        tareaActual.PrioridadTarea = tarea.PrioridadTarea;
        tareaActual.Descripcion = tarea.Descripcion;

        await dbContext.SaveChangesAsync();

        return Results.Ok();

    }

    return Results.NotFound();   
});

app.MapDelete("/api/tareas/{id}", async ([FromServices] TareasContext dbContext, [FromRoute] Guid id) =>
{
    var tareaActual = dbContext.Tareas.Find(id);

   if(tareaActual!=null)
     {
         dbContext.Remove(tareaActual);
         await dbContext.SaveChangesAsync();

         return Results.Ok();
     }

    return Results.NotFound();
});

app.Run();