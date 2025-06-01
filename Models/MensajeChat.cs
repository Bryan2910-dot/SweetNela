using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SweetNela.Models
{
    public class MensajeChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ContactoId { get; set; } // Relación al contacto

        [Required]
        public string Remitente { get; set; } // "Admin" o "Contacto"

        [Required]
        public string Contenido { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow; // Siempre en UTC

        [ForeignKey("ContactoId")]
        public Contacto Contacto { get; set; } // Navegación
    }
}
