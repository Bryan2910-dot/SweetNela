using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;
using SweetNela.MLintegration;// Namespace del modelo generado por ML.NET
using System.Linq;
using System.Threading.Tasks;

namespace SweetNela.Controllers
{
    [Authorize]
    public class RecomendacionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RecomendacionesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Devuelve una lista de productos recomendados ordenados por score
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var productos = await _context.DbSetProducto.ToListAsync();
            var recomendaciones = new List<(Producto, float)>(); // Producto + Score

            foreach (var producto in productos)
            {
                var sampleData = new ProductRecommender.ModelInput
                {
                    UserId = userId,
                    ProductId = producto.Id
                };

                var prediction = ProductRecommender.Predict(sampleData);
                recomendaciones.Add((producto, prediction.Score));
            }

            // Ordenar por score descendente y tomar los top 5
            var topRecomendados = recomendaciones
                .OrderByDescending(r => r.Item2)
                .Take(5)
                .Select(r => r.Item1)
                .ToList();

            return View(topRecomendados);
        }
    }
}
