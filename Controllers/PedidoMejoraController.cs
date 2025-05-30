using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SweetNela.Data;
using SweetNela.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SweetNela.Controllers
{
    
    public class PedidoMejoraController : Controller
    {
        private readonly ILogger<PedidoMejoraController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PedidoMejoraController(ILogger<PedidoMejoraController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CalcularPrecio([FromBody] PedidoMejora pedido)
        {
            if (pedido == null)
                return BadRequest();

            var resultado1 = pedido.Calcular1();
            var resultado2 = pedido.Calcular2();
            var resultado3 = pedido.Calcular3();
            var sumaTotal = resultado1 + resultado2 + resultado3;

            return Json(new { precio = sumaTotal });
        }
        [HttpPost]
        public IActionResult PedidoMejora(PedidoMejora pedido)
        {
            if (ModelState.IsValid)
            {
                string mensaje = "";
                try
                {
                    // Calcula los valores
                    var resultado1 = pedido.Calcular1();
                    var resultado2 = pedido.Calcular2();
                    var resultado3 = pedido.Calcular3();

                    // Suma los resultados
                    pedido.SumaTotal = resultado1 + resultado2 + resultado3;

                    // Guarda el pedido en la tabla PedidoMejora
                    _context.DbSetPedidoMejora.Add(pedido);
                    _context.SaveChanges();

                    // Crea una nueva entrada para la tabla PreOrden
                    var preOrden = new PreOrden
                    {
                        Precio = (decimal)pedido.SumaTotal, // Asigna SumaTotal al atributo Precio
                        UserName = _userManager.GetUserName(User),          // Asigna el valor de Lugar al atributo UserName
                        Cantidad = 1,                      // Puedes ajustar la cantidad según sea necesario
                        Status = "PENDIENTE"               // Estado inicial
                    };

                    // Guarda en la tabla PreOrden
                    _context.DbSetPreOrden.Add(preOrden);
                    _context.SaveChanges();

                    // Prepara el mensaje para mostrar en el HTML
                    mensaje = $"El pedido se ha creado correctamente con un total de: {pedido.SumaTotal}";
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    mensaje = $"Ocurrió un error: {ex.Message}";
                }

                // Envía el resultado al HTML
                ViewData["Resultado"] = mensaje;
            }
            else
            {
                ViewData["Resultado"] = "Datos de entrada no válidos";
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}