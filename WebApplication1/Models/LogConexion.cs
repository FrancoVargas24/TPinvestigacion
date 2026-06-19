using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class LogConexion
    {
        [Key]
        public int LogConexionId { get; set; }

        // FK Usuario
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty; // "Conexion" o "Desconexion"

        public DateTime Fecha { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? ConnectionId { get; set; } // Id de conexión de SignalR
    }
}
