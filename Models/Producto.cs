using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SweetNela.Models
{
    [Table("t_producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotNull]
        public string? Name { get; set; }
        [NotNull]
        public Decimal Price { get; set; }
        [NotNull]
        public string? Status { get; set; }
        [NotNull]
        public string? ImageURL { get; set; }
    }
}