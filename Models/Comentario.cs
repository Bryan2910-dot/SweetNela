using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SweetNela.Models
{
    [Table("t_comentario")]
    public class Comentario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Orden")]
        public int OrdenId { get; set; }
        public Orden? Orden { get; set; }

        [ForeignKey("DetalleOrden")]
        public int DetalleOrdenId { get; set; }
        public DetalleOrden? DetalleOrden { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }

        [Column("comentario")]
        [Required]
        [MaxLength(1000)]
        public string Texto { get; set; } = string.Empty;

        [Column("sentimiento")]
        [MaxLength(50)]
        public string? Sentimiento { get; set; } // Positivo o Negativo

        [Column("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
