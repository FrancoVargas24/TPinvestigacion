namespace Entidades;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Enums;

public class Publicacion
{
    public int Id { get; set; }

    [Required]
    public string Titulo { get; set; }
    [Required]
    public string Descripcion { get; set; }
    [Required]
    public string ImagenUrl { get; set; }

    // va ser la fecha de inicio de subasta-permuta para no agregar logica tenemos menos de 5 dias para hacer el proyecto 
    public DateTime FechaCreacion { get; set; }
    [Required]
    public DateTime FechaCierre { get; set; }

    public EstadoPublicacion Estado { get; set; }


    [ForeignKey(nameof(Usuario))]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    [ForeignKey(nameof(Categoria))]
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; }

    public ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
}