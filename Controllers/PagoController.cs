using Microsoft.AspNetCore.Mvc;
using SweetNela.Data;
using SweetNela.Models;
using SweetNela.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SweetNela.Controllers;

public class PagoController : Controller
{
    private readonly PayPalService _payPalService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PagoController(PayPalService payPalService, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _payPalService = payPalService;
        _context = context;
        _userManager = userManager;
    }

        [HttpGet]
        public IActionResult Resumen(decimal monto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("Debe iniciar sesión para proceder con el pago.");
            }

            var itemsCarrito = _context.DbSetPreOrden
                .Include(p => p.Producto) // Asegúrate de incluir la relación Producto
                .Where(p => p.UserId == userId && p.Status == "PENDIENTE")
                .ToList();

            if (!itemsCarrito.Any())
            {
                return BadRequest("El carrito está vacío.");
            }

            ViewData["MontoTotal"] = monto;
            return View(itemsCarrito); // Pasa el modelo correctamente
        }

        [HttpGet]
        public async Task<IActionResult> Create(decimal monto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized("Debe iniciar sesión para realizar un pago.");
            }

            // Crear el pago en PayPal
            var approvalUrl = await _payPalService.CreatePaymentAsync(monto, "PEN",
                Url.Action("PaymentSuccess", "Pago", null, Request.Scheme),
                Url.Action("PaymentCancel", "Pago", null, Request.Scheme));

            return Redirect(approvalUrl);
        }

        public async Task<IActionResult> PaymentSuccess(string paymentId, string PayerID)
        {
            var userId = _userManager.GetUserId(User);

            // Registrar el pago en la base de datos
            var pago = new Pago
            {
                PaymentDate = DateTime.UtcNow,
                MontoTotal = _context.DbSetPreOrden
                    .Where(p => p.UserId == userId && p.Status == "PENDIENTE")
                    .Sum(p => p.Cantidad * p.Precio),
                Status = "Exitoso",
                PayPalPaymentId = paymentId,
                PayPalPayerId = PayerID,
                UserId = userId
            };

            _context.DbSetPago.Add(pago);

            // Actualizar el estado de los productos en el carrito
            var itemsCarrito = _context.DbSetPreOrden
                .Where(p => p.UserId == userId && p.Status == "PENDIENTE")
                .ToList();

            foreach (var item in itemsCarrito)
            {
                item.Status = "PROCESADO";
            }

            _context.UpdateRange(itemsCarrito);
            await _context.SaveChangesAsync();

            return Ok("Pago realizado con éxito.");
        }

    [HttpGet]
    public async Task<IActionResult> TestPayPal()
    {
        var result = await _payPalService.TestPayPalCredentialsAsync();
        return Content(result);
    }

    public IActionResult PaymentCancel()
    {
        return BadRequest("El pago fue cancelado.");
    }
}