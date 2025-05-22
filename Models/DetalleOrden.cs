using System.ComponentModel.DataAnnotations.Schema;

namespace SweetNela.Models
{
    [Table("t_order_detail")]
    public class DetalleOrden
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public Producto? Producto { get; set; }

        public int Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public Orden? Orden { get; set; }
    }
}