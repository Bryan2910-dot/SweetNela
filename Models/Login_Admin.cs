using System.ComponentModel.DataAnnotations;

namespace SweetNela.Models
{
    public class Login_Admin
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string? AdminNombre { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string? AdminContra { get; set; }
    }
}
