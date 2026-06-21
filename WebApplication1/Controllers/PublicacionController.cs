using System.Security.Claims;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace WebApplication1.Controllers
{
    public class PublicacionController : Controller
    {
        private readonly IPublicacionService _publicacionService;
        private readonly ICategoriaService _categoriaService;
        private readonly IWebHostEnvironment _environment;


        public PublicacionController(IPublicacionService publicacionService, ICategoriaService categoriaService, IWebHostEnvironment environment)
        {
            _publicacionService = publicacionService;
            _categoriaService = categoriaService;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var publicaciones = await _publicacionService.ObtenerActivasAsync();
            return View(publicaciones);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var publicacion = await _publicacionService.ObtenerPorIdAsync(id);
            if (publicacion == null) return NotFound();
            return View(publicacion);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ViewBag.Categorias = await _categoriaService.ObtenerTodasAsync();
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            [Bind("Titulo,Descripcion,FechaCierre,CategoriaId")] Publicacion publicacion,
            IFormFile imagenArchivo)
        {
            if (imagenArchivo == null || imagenArchivo.Length == 0)
            {
                ModelState.AddModelError("", "Tenés que subir una imagen");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _categoriaService.ObtenerTodasAsync();
                return View(publicacion);
            }

            publicacion.ImagenUrl = await GuardarImagenAsync(imagenArchivo!);
            publicacion.UsuarioId = ObtenerUsuarioIdLogueado();

            await _publicacionService.CrearAsync(publicacion);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GuardarImagenAsync(IFormFile archivo)
        {
            var carpeta = Path.Combine(_environment.WebRootPath, "images", "publicaciones");
            Directory.CreateDirectory(carpeta);

            var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return $"/images/publicaciones/{nombreArchivo}";
        }


        private int ObtenerUsuarioIdLogueado()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(idClaim!);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var publicacion = await _publicacionService.ObtenerPorIdAsync(id);
            if (publicacion == null) return NotFound();

            if (publicacion.UsuarioId != ObtenerUsuarioIdLogueado())
                return Forbid();

            ViewBag.Categorias = await _categoriaService.ObtenerTodasAsync();
            return View(publicacion);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            int id,
            [Bind("Id,Titulo,Descripcion,FechaCierre,CategoriaId")] Publicacion publicacionForm,
            IFormFile? imagenArchivo)
        {
            if (id != publicacionForm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Categorias = await _categoriaService.ObtenerTodasAsync();
                return View(publicacionForm);
            }

            string? nuevaImagenUrl = null;
            if (imagenArchivo != null && imagenArchivo.Length > 0)
            {
                nuevaImagenUrl = await GuardarImagenAsync(imagenArchivo);
            }

            var exito = await _publicacionService.EditarAsync(id, ObtenerUsuarioIdLogueado(), publicacionForm, nuevaImagenUrl);
            if (!exito) return Forbid();

            return RedirectToAction(nameof(Details), new { id });
        }

    }
}