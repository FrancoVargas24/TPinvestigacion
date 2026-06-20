using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Entidades;
public class Mensaje
{
    public int Id { get; set; }

    public string Texto { get; set; }

    public DateTime FechaEnvio { get; set; }

    public bool EsPrivado{get; set;} //false= chat grupal - true= chat privado

    [ForeignKey(nameof(Usuario))]
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    [ForeignKey(nameof(Publicacion))]
    public int PublicacionId { get; set; }
    public Publicacion Publicacion { get; set; }

    //se lo completa cuando en el caso de chat privado entre 2
    public int? DestinatarioId {get; set;}
    public Usuario? Destinatario {get; set;}
}