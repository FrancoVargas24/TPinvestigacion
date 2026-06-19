using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingresá tu usuario o email")]
        [Display(Name = "Usuario o Email")]
        public string? UsuarioOEmail { get; set; }

        [Required(ErrorMessage = "Ingresá tu contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }
    }
}
