namespace Entidades;

using System.ComponentModel.DataAnnotations.Schema;
using Enums;
public class Oferta
{
    public int Id { get; set; }

    public TipoOferta Tipo { get; set; }

    public string Detalle { get; set; }

    public EstadoOferta Estado { get; set; }

    public DateTime FechaOferta { get; set; }

    [ForeignKey(nameof(Usuario))]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    [ForeignKey(nameof(Publicacion))]
    public int PublicacionId { get; set; }
    public Publicacion Publicacion { get; set; }
}