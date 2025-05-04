using System.ComponentModel.DataAnnotations;

namespace ECommerce.Controllers.DTOs
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de usuario")]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 3)]
        public required string Username { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [Display(Name = "Correo electrónico")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de contraseña no coinciden.")]
        public required string ConfirmPassword { get; set; }
    }
}
