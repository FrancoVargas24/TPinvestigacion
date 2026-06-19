using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
        public class RegistroViewModel
        {
            [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
            [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener entre 3 y 50 caracteres")]
            [Display(Name = "Nombre de usuario")]
            public string? NombreUsuario { get; set; }

            [Required(ErrorMessage = "El email es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato de email no es válido")]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string? Password { get; set; }

            [Required(ErrorMessage = "Confirmá tu contraseña")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
            public string? ConfirmarPassword { get; set; }
        }
}