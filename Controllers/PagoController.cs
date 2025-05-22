using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;

namespace SweetNela.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

           public PagoController(ILogger<PagoController> logger,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Create(decimal monto)
        {

            Pago pago = new Pago();
            pago.UserName = _userManager.GetUserName(User);
            pago.MontoTotal = monto;
            _logger.LogInformation("El monto es: ${monto}", pago.MontoTotal.ToString());
            return View(pago);
        }


[HttpPost]
public IActionResult Pagar(Pago pago)
{
    pago.PaymentDate = DateTime.UtcNow;
    pago.Status = "CANCELADO"; // ✅ Ponle el status ANTES de guardar

    _context.Add(pago); // EF lo está rastreando como "Added"

    var itemsCarrito = from o in _context.DbSetPreOrden select o;
    itemsCarrito = itemsCarrito
        .Include(p => p.Producto)
        .Where(s => s.UserName.Equals(pago.UserName) && s.Status.Equals("PENDIENTE"));

    Orden pedido = new Orden
    {
        UserName = pago.UserName,
        Total = pago.MontoTotal,
        Pago = pago,
        Status = "PENDIENTE"
    };

    _context.Add(pedido);

    List<DetalleOrden> itemsPedido = new();
    foreach (var item in itemsCarrito.ToList())
    {
        itemsPedido.Add(new DetalleOrden
        {
            Orden = pedido,
            Precio = item.Precio,
            Producto = item.Producto,
            Cantidad = item.Cantidad
        });
    }

    _context.AddRange(itemsPedido);

    foreach (PreOrden p in itemsCarrito.ToList())
    {
        p.Status = "PROCESADO";
    }

    _context.UpdateRange(itemsCarrito);

    // No es necesario llamar a _context.Update(pago); EF lo rastrea como "Added"
    _context.SaveChanges();

    ViewData["Message"] = $"El pago se ha registrado y su pedido nro {pedido.Id} está en camino";
return View("Create", pago);
}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
}
