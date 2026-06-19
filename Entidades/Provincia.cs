using System.ComponentModel.DataAnnotations;

namespace Entidades;
public class Provincia
{
    [Key]
    public int Id {get; set;}
    public string Nombre {get; set;}

}