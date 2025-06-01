using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SweetNela.Models
{
    [Table("t_order")]
    public class Orden
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public string? UserId { get; set; }

        public Decimal Total { get; set; }

        public DateTime Fecha { get; set; }

        public Pago? Pago { get; set; }


        public string? Status { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
    }
}