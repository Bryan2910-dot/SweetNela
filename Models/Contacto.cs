using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add this namespace for validation
using System.Linq;
using System.Threading.Tasks;

namespace Sweetnela.Models
{
    public class Contacto
    {
        [Required(ErrorMessage = "Debe llenar este campo")]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Debe llenar este campo")]
        public string? Mensaje { get; set; }
        
        public string? Respuesta { get; set; }
    }
}