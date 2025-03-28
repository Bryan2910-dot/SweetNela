using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SweetNela.Models;

namespace SweetNela.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ILogger<PedidoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Pedido(Pedido calc1)
        {
            if (ModelState.IsValid)
            {
                string mensaje = "";
                try
                {
                    // Llama a los tres métodos de cálculo del modelo Pedido
                    var resultado1 = calc1.Calcular1();
                    var resultado2 = calc1.Calcular2();
                    var resultado3 = calc1.Calcular3();

                    // Suma los resultados
                    var sumaTotal = resultado1 + resultado2 + resultado3;

                    // Prepara el mensaje para mostrar en el HTML
                    mensaje = $"El resultado de los cálculos es: {sumaTotal}";
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