using Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class Oferta
    {
        public int Id { get; set; }

        public decimal? Monto { get; set; }

        public string? ProductoOfrecido { get; set; }

        public EstadoOferta Estado { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int PublicacionId { get; set; }
        public Publicacion Publicacion { get; set; }
    }
}
