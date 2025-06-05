using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SweetNela.Data;
using SweetNela.Dto;
using SweetNela.Integration.Exchange;
using SweetNela.Models;

namespace SweetNela.Controllers;

public class CatalogoController : Controller
{
    private readonly ILogger<CatalogoController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ExchangeIntegration _exchange;


public CatalogoController(
    ILogger<CatalogoController> logger,
    ApplicationDbContext context,
    ExchangeIntegration exchange)
{
    _logger = logger;
    _context = context;
    _exchange = exchange;
}

[AllowAnonymous]
public async Task<IActionResult> Index()
{
    var productos = _context.DbSetProducto.ToList();

    var tipoCambio = new TipoCambio
    {
        From = "PEN",
        To = "USD",
        Cantidad = 1
    };

    double rate = await _exchange.GetExchangeRate(tipoCambio);

    ViewData["TipoCambioUSD"] = rate;
    _logger.LogInformation("Tipo de cambio PEN -> USD: {rate}", rate);
    
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
