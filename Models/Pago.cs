using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SweetNela.Data;

namespace SweetNela.Models
{
    [Table("t_pago")]
    public class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Identificador único del pago

        public DateTime PaymentDate { get; set; } // Fecha del pago

        public Decimal MontoTotal { get; set; } // Monto total pagado

        public string? Status { get; set; } // Estado del pago (e.g., "Exitoso", "Fallido")

        public string? PayPalPaymentId { get; set; } // ID del pago generado por PayPal
        public string? PayPalPayerId { get; set; } // ID del pagador en PayPal

        // Relación con t_order
        public int? OrderId { get; set; } // Clave foránea
        [ForeignKey("OrderId")]
        public Orden? Order { get; set; } // Propiedad de navegación
        
        // Relación con el usuario
        public string? UserId { get; set; } // ID del usuario que realizó el pago
        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; } // Propiedad de navegación con AspNetUsers
    }
}