using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.ML;

namespace SweetNela.Controllers
{
    [Authorize]
    public class ComentarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PredictionEnginePool<MLSentimiento.MLSentimiento.ModelInput, MLSentimiento.MLSentimiento.ModelOutput> _predictionEngine;

        public ComentarioController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            PredictionEnginePool<MLSentimiento.MLSentimiento.ModelInput, MLSentimiento.MLSentimiento.ModelOutput> predictionEngine)
        {
            _context = context;
            _userManager = userManager;
            _predictionEngine = predictionEngine;
        }



        // GET: /Comentario
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var detallesSinComentario = await _context.DbSetDetalleOrden
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .Where(d => d.Orden!.UserId == user.Id && d.Comentario == null)
                .ToListAsync();

            return View(detallesSinComentario);
        }

        // GET: /Comentario/Crear/5
        public async Task<IActionResult> Crear(int id) // id = DetalleOrdenId
        {
            var detalle = await _context.DbSetDetalleOrden
                .Include(d => d.Producto)
                .Include(d => d.Orden)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detalle == null || detalle.Comentario != null)
            {
                return NotFound();
            }

            ViewBag.DetalleOrden = detalle;
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Crear(int detalleOrdenId, string texto, int rating)
{
    var user = await _userManager.GetUserAsync(User);
    var detalle = await _context.DbSetDetalleOrden
        .Include(d => d.Producto)
        .Include(d => d.Orden)
        .FirstOrDefaultAsync(d => d.Id == detalleOrdenId);

    if (detalle == null || detalle.Comentario != null)
    {
        return NotFound();
    }

    // Imprimir el texto recibido (puedes usar logs si tienes)
    Console.WriteLine($"Texto recibido para predicción: {texto}");

    // Crear input para el modelo ML
    var input = new MLSentimiento.MLSentimiento.ModelInput
    {
        Text = texto
    };

    // Ejecutar la predicción
    var result = _predictionEngine.Predict(input);

    // Imprimir toda la salida del modelo para revisar
    Console.WriteLine($"PredictedLabel: {result.PredictedLabel}");
    if (result.Score != null)
    {
        Console.WriteLine($"Score: {string.Join(", ", result.Score)}");
    }

    var sentimiento = result.PredictedLabel;

    // Guardar comentario en BD
    var comentario = new Comentario
    {
        DetalleOrdenId = detalleOrdenId,
        OrdenId = detalle.Orden!.Id,
        UserId = user!.Id,
        Texto = texto,
        Sentimiento = sentimiento,
        Rating = rating
    };

    _context.DbComentario.Add(comentario);
    await _context.SaveChangesAsync();

    ViewBag.MensajeSentimiento = $"Gracias por tu comentario. Se detectó un sentimiento: {sentimiento}.";
    ViewBag.DetalleOrden = detalle;

    // Evitar que vuelva a comentar
    ViewBag.ComentarioEnviado = true;

    return View();
}



    }
}
