using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Models;
using SweetNela.Service;
using SweetNela.Data;
namespace SweetNela.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly PayPalService _payPalService;
 
        public PagoController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            PayPalService payPalService)
        {
            _userManager = userManager;
            _context = context;
            _payPalService = payPalService;
        }
 
        // GET: /Pago/Resumen
        public async Task<IActionResult> Resumen()
        {
            // Obtener la pre-orden del usuario actual (ajusta según tu lógica)
            var userId = _userManager.GetUserId(User);
            var preOrdenes = await _context.DbSetPreOrden
                .Include(p => p.Producto)
                .Where(p => p.UserId == userId)
                .ToListAsync();
 
            var montoTotal = preOrdenes.Sum(p => p.Precio * p.Cantidad);
            ViewData["MontoTotal"] = montoTotal;
 
            return View(preOrdenes);
        }
 
        // GET: /Pago/Create?monto=xx
        public async Task<IActionResult> Create(decimal monto)
        {
            var userId = _userManager.GetUserId(User);
 
            // Aquí deberías crear la Orden y guardarla antes de crear el pago
            var orden = new Orden
            {
                UserId = userId,
                Fecha = DateTime.UtcNow,
                Total = monto
                // Agrega otros campos necesarios
            };
            _context.DbSetOrden.Add(orden);
            await _context.SaveChangesAsync();
 
            var returnUrl = Url.Action("Success", "Pago", null, Request.Scheme);
            var cancelUrl = Url.Action("Cancel", "Pago", null, Request.Scheme);
            var approvalUrl = await _payPalService.CreatePaymentAsync(monto, "USD", returnUrl, cancelUrl);
            // Registrar el pago en la base de datos
            var pago = new Pago
            {
                MontoTotal = monto,
                UserId = userId,
                OrderId = orden.Id,
                Status = "Pending"
            };
            _context.DbSetPago.Add(pago);
            await _context.SaveChangesAsync();
 
            ViewData["ApprovalUrl"] = approvalUrl;
            return View(pago);
        }
 
        // GET: /Pago/Success
        public async Task<IActionResult> Success(string paymentId, string PayerID)
        {
            // Obtener el usuario actual
            var userId = _userManager.GetUserId(User);
 
            // Buscar el pago pendiente
            var pago = await _context.DbSetPago
                .Where(p => p.UserId == userId && p.Status == "Pending")
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();
 
            if (pago != null)
            {
 
                // Actualizar el estado del pago
                pago.Status = "Completed";
                pago.PayPalPaymentId = paymentId;
                pago.PayPalPayerId = PayerID;
                await _context.SaveChangesAsync();
                var ordenFinal = await _context.DbSetOrden.FindAsync(pago.OrderId);
                var itemsCarrito = from o in _context.DbSetPreOrden select o;
                itemsCarrito = itemsCarrito
                    .Include(p => p.Producto)
                    .Where(s => s.UserId.Equals(userId) && s.Status.Equals("PENDIENTE"));
                // Crear pago en PayPal
 
                List<DetalleOrden> itemsPedido = new();
                foreach (var item in itemsCarrito.ToList())
                {
                    itemsPedido.Add(new DetalleOrden
                    {
                        Orden = ordenFinal,
                        Precio = item.Precio,
                        Producto = item.Producto,
                        Cantidad = item.Cantidad
                    });
                }
 
                _context.AddRange(itemsPedido);
 
 
                // Vaciar el carrito del usuario
                var preOrdenes = _context.DbSetPreOrden.Where(p => p.UserId == userId);
                _context.DbSetPreOrden.RemoveRange(preOrdenes);
                await _context.SaveChangesAsync();
            }
 
            ViewData["Message"] = "¡Pago realizado con éxito!";
            return View("Success", pago);
        }
 
        // GET: /Pago/Cancel
        public IActionResult Cancel()
        {
            ViewData["Message"] = "El pago fue cancelado.";
            return View("Cancel");
        }
 
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TestPayPal()
        {
            var result = await _payPalService.TestPayPalCredentialsAsync();
            return Content(result);
        }
    }
}
 
