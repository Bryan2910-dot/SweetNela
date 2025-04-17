using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;

namespace SweetNela.Controllers;

public class CarritoController : Controller
{
    private readonly ILogger<CarritoController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CarritoController(ILogger<CarritoController> logger,
         UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userIDSession = _userManager.GetUserName(User);
            if (userIDSession == null)
            {
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                return RedirectToAction("Index", "Catalogo");
            }
            var items = from o in _context.DbSetPreOrden select o;
            items = items.Include(p => p.Producto).
                    Where(w => w.UserName.Equals(userIDSession) &&
                        w.Status.Equals("PENDIENTE"));
            var itemsCarrito = items.ToList();
            var total = itemsCarrito.Sum(c => c.Cantidad * c.Precio);

            dynamic model = new ExpandoObject();
            model.montoTotal = total;
            model.elementosCarrito = itemsCarrito;
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Add(int? id)
        {
            var userID = _userManager.GetUserName(User);
            if (userID == null)
            {
                _logger.LogInformation("No existe usuario");
                ViewData["Message"] = "Por favor debe loguearse antes de agregar un producto";
                return RedirectToAction("Index", "Catalogo");
            }
            else
            {
                var producto = await _context.DbSetProducto.FindAsync(id);
                PreOrden proforma = new PreOrden();
                proforma.Producto = producto;
                proforma.Precio = producto.Price;
                proforma.Cantidad = 1;
                proforma.UserName = userID;
                _context.Add(proforma);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Se Agrego al carrito";
                _logger.LogInformation("Se agrego un producto al carrito");
                return RedirectToAction("Index", "Catalogo");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var preorden = await _context.DbSetPreOrden.FindAsync(id);
            if (preorden != null)
            {
                _context.DbSetPreOrden.Remove(preorden);
                await _context.SaveChangesAsync();
                ViewData["Message"] = "Se elimino del carrito";
                _logger.LogInformation("Se elimino un producto del carrito");
            }
            return RedirectToAction("Index", "Carrito");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var itemPreOrden = await _context.DbSetPreOrden.FindAsync(id);
            if (itemPreOrden == null)
            {
                return NotFound();
            }
            return View(itemPreOrden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cantidad,Precio,UserID")] PreOrden itemCarrito)
        {
            if (id != itemCarrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCarrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DbSetPreOrden.Any(e => e.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCarrito);
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
