using System.Diagnostics;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Dto;
using SweetNela.Integration.Exchange;
using SweetNela.Models;

namespace SweetNela.Controllers{
    public class TipoCambioController : Controller
    {
        private readonly ILogger<TipoCambioController> _logger;
        private readonly ExchangeIntegration _exchange;

        public TipoCambioController(ILogger<TipoCambioController> logger,
        ExchangeIntegration exchange)
        {
            _logger = logger;
            _exchange = exchange;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Exchange(TipoCambio? tipoCambio)
        {
            double rate = await _exchange.GetExchangeRate(tipoCambio);
            var cambio = tipoCambio.Cantidad * rate;
            _logger.LogInformation($"Tipo de cambio de {tipoCambio.From} a {tipoCambio.To} es {rate} y cambio {cambio}");
            ViewData["rate"] = String.Format("{0:F2}", rate);
            ViewData["cambio"] = String.Format("{0:N2}", cambio); ;
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}