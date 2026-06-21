using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Servicios
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ObtenerTodasAsync();
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly MusicTradeDbContext _context;

        public CategoriaService(MusicTradeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> ObtenerTodasAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}
