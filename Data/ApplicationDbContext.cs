using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SweetNela.Models;

namespace SweetNela.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Contacto> DbSetContacto { get; set; }

    }
    public DbSet<PedidoMejora> DbSetPedidoMejora { get; set; }

}

