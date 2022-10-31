using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace proyectoef.Models;

public class Tarea
{
    //[Key]//la clave de la tabla Tareas
    public Guid TareaId {get;set;}

    //[ForeignKey("CategoriaId")]//la llave foranea o relacion que hay entre la tabla categoria y tarea
    public Guid CategoriaId {get;set;}

    //[Required]
    //[MaxLength(200)]
    public string Titulo {get;set;}

    public string Descripcion {get;set;}

    public Prioridad PrioridadTarea {get;set;}

    public DateTime FechaCreacion {get;set;}
    
    public virtual Categoria Categoria {get;set;}
    
    //[NotMappedAttribute]
    public string Resumen {get;set;}
}

public enum Prioridad
{
    Baja,
    Media,
    Alta
}