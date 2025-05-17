using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SweetNela.Models;

namespace SweetNela.Data{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PedidoMejora> DbSetPedidoMejora { get; set; }
        public DbSet<Contacto> DbSetContacto { get; set; }
        public DbSet<Producto> DbSetProducto { get; set; }
        public DbSet<PreOrden> DbSetPreOrden { get; set; }


    }

    
}


