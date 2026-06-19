using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades;
public class Mensaje
{
    public int Id { get; set; }

    public string Texto { get; set; }

    public DateTime FechaEnvio { get; set; }

    [ForeignKey(nameof(Usuario))]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    [ForeignKey(nameof(Publicacion))]
    public int PublicacionId { get; set; }
    public Publicacion Publicacion { get; set; }
}