using System.Security.Claims;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace WebApplication1.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IConversacionService _conversacionService;

    public ChatController(IConversacionService conversacionService)
    {
        _conversacionService = conversacionService;
    }

    public async Task<IActionResult> Index()
    {
        int usuarioId = ObtenerUsuarioIdLogueado();

        var conversaciones =
            await _conversacionService.ObtenerConversacionesUsuarioAsync(usuarioId);

        return View(conversaciones);
    }

    public async Task<IActionResult> Conversacion(int id)
    {
        var conversacion =
            await _conversacionService.ObtenerConversacionAsync(id);

        if (conversacion == null)
            return NotFound();

        return View(conversacion);
    }

    [HttpPost]
    public async Task<IActionResult> EnviarMensaje(int conversacionId, string texto)
    {
        if (!string.IsNullOrWhiteSpace(texto))
        {
            await _conversacionService.EnviarMensajeAsync(
                conversacionId,
                ObtenerUsuarioIdLogueado(),
                texto);
        }

        return RedirectToAction(nameof(Conversacion), new { id = conversacionId });
    }

    public async Task<IActionResult> CrearConversacion(int publicacionId)
    {
        var conversacion =
            await _conversacionService.CrearConversacionAsync(
                publicacionId,
                ObtenerUsuarioIdLogueado());

        return RedirectToAction(nameof(Conversacion),
            new { id = conversacion.Id });
    }

    public async Task<IActionResult> AceptarOferta(int publicacionId, int ofertanteId)
    {
        var conversacion =
            await _conversacionService.CrearConversacionAsync(
                publicacionId,
                ofertanteId);

        return RedirectToAction(nameof(Conversacion),
            new { id = conversacion.Id });
    }

    private int ObtenerUsuarioIdLogueado()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}