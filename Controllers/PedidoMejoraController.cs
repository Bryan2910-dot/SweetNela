using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SweetNela.Data;
using SweetNela.Models;

namespace SweetNela.Controllers
{
    
    public class PedidoMejoraController : Controller
    {
        private readonly ILogger<PedidoMejoraController> _logger;
        private readonly ApplicationDbContext _context;

        public PedidoMejoraController(ILogger<PedidoMejoraController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PedidoMejora(PedidoMejora calc1)
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
                    // Asigna la suma total a la propiedad del modelo
                    calc1.SumaTotal = sumaTotal;
                    // Guarda el modelo en la base de datos
                    _context.DbSetPedidoMejora.Add(calc1);
                    _context.SaveChanges();

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