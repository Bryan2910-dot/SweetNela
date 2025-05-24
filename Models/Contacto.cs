using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SweetNela.Models
{
    [Table("t_contacto")]
    public class Contacto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        public string? Telefono { get; set; }

        // Aquí quito [Required] para que sea opcional
        public string? Mensaje { get; set; }

        public string? Respuesta { get; set; }

        [ValidateNever]
        public ICollection<MensajeChat> Mensajes { get; set; }
    }
}

