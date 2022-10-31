
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace proyectoef.Models;

//clases
public class Categoria
{

    //[Key]
    //Atributos
    public Guid CategoriaId {get;set;}

    //[Required]//forzar a que este atributo sea requerido para la base de datos
    //[MaxLength(150)]//maximo de caracteres admitidos
    public string Nombre {get;set;}


    public string Descripcion {get;set;}

    public int Peso {get;set;}

    //Relacion con tareas
    [JsonIgnore]//Al momento de llamar la categoria en el TareaContext se debe ignorar esto ya que se crea un ciclo infinito
    public virtual ICollection<Tarea> Tareas {get;set;}
}