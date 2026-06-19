using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;

        public CuentaController(ApplicationDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        // GET: /Cuenta/Registro
        [HttpGet]
        public IActionResult Registro()
        {
            return View(new RegistroViewModel());
        }

        // POST: /Cuenta/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Verificar que no exista ya el usuario o el email
            bool existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == modelo.NombreUsuario);

            if (existeUsuario)
            {
                ModelState.AddModelError(nameof(modelo.NombreUsuario), "Ese nombre de usuario ya está en uso");
            }

            bool existeEmail = await _context.Usuarios
                .AnyAsync(u => u.Email == modelo.Email);

            if (existeEmail)
            {
                ModelState.AddModelError(nameof(modelo.Email), "Ese email ya está registrado");
            }

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = modelo.NombreUsuario!,
                Email = modelo.Email!,
                PasswordHash = _passwordService.HashPassword(modelo.Password!),
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "¡Cuenta creada con éxito! Ya podés iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /Cuenta/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Cuenta/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.NombreUsuario == modelo.UsuarioOEmail || u.Email == modelo.UsuarioOEmail);

            if (usuario == null || !_passwordService.VerifyPassword(modelo.Password!, usuario.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Usuario/email o contraseña incorrectos");
                return View(modelo);
            }

            if (!usuario.Activo)
            {
                ModelState.AddModelError(string.Empty, "Esta cuenta está inactiva");
                return View(modelo);
            }

            // Crear los claims de la sesión
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            // Log de conexión (una de nuestras 5 entidades en uso real)
            _context.LogsConexion.Add(new LogConexion
            {
                UsuarioId = usuario.UsuarioId,
                Tipo = "Conexion",
                Fecha = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // POST: /Cuenta/Logout
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var usuarioIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(usuarioIdStr, out int usuarioId))
            {
                _context.LogsConexion.Add(new LogConexion
                {
                    UsuarioId = usuarioId,
                    Tipo = "Desconexion",
                    Fecha = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
