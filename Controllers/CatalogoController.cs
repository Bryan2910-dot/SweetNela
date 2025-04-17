using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetNela.Data;
using SweetNela.Models;

namespace SweetNela.Controllers;

public class CatalogoController : Controller
{
    private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;

        public CatalogoController(ILogger<CatalogoController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var productos = _context.DbSetProducto.ToList();
            _logger.LogInformation("Productos: {0}", productos);
            return View(productos);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            Producto objProduct = await _context.DbSetProducto.FindAsync(id);
            if (objProduct == null)
            {
                return NotFound();
            }
            return View(objProduct);
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
