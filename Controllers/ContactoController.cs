using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SweetNela.Data;
using SweetNela.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // <- Importante para UserManager

namespace SweetNela.Controllers
{
    public class ContactoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // <- Nuevo

        public ContactoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager; // <- Nuevo
        }

        // GET: Contacto
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.DbSetContacto.ToListAsync());
        }

        // GET: Contacto/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.DbSetContacto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // GET: Contacto/Create
        [Authorize]
public async Task<IActionResult> Create()
{
    var user = await _userManager.GetUserAsync(User);
    var contacto = await _context.DbSetContacto
        .Include(c => c.Mensajes)
        .FirstOrDefaultAsync(c => c.Email == user.Email);

    if (contacto == null)
    {
        contacto = new Contacto
        {
            Email = user.Email,
            Telefono = user.PhoneNumber,
            Nombres = user.UserName
        };
        _context.DbSetContacto.Add(contacto);
        await _context.SaveChangesAsync();
    }

    return View(contacto);
}
[HttpPost]
[Authorize]
public async Task<IActionResult> EnviarMensajeUsuario(int contactoId, string contenido)
{
    if (string.IsNullOrWhiteSpace(contenido))
        return RedirectToAction("Create");

    var user = await _userManager.GetUserAsync(User);
    var mensaje = new MensajeChat
    {
        ContactoId = contactoId,
        Remitente = user.Email,
        Contenido = contenido,
        FechaEnvio = DateTime.UtcNow
    };

    _context.DbSetMensajeChat.Add(mensaje);
    await _context.SaveChangesAsync();

    return RedirectToAction("Create");
}


        // POST: Contacto/Create
        // POST: Contacto/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Nombres,Email,Telefono,Mensaje,Respuesta")] Contacto contacto)
{
    if (ModelState.IsValid)
    {
        _context.Add(contacto);
        await _context.SaveChangesAsync();

        TempData["MensajeEnviado"] = "¡Mensaje enviado correctamente!";
        return RedirectToAction(nameof(Create));
    }
    
    // Si hay errores, listarlos para debug
    var errores = string.Join("; ", ModelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage));
    ViewBag.ErroresValidacion = errores;

    return View(contacto);
}



        // GET: Contacto/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.DbSetContacto.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }
            return View(contacto);
        }

        // POST: Contacto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Email,Telefono,Mensaje,Respuesta")] Contacto contacto)
        {
            if (id != contacto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contacto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactoExists(contacto.Id))
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
            return View(contacto);
        }

        // GET: Contacto/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.DbSetContacto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // POST: Contacto/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contacto = await _context.DbSetContacto.FindAsync(id);
            if (contacto != null)
            {
                _context.DbSetContacto.Remove(contacto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
public async Task<IActionResult> Chat(int id)
{
    var contacto = await _context.DbSetContacto
        .Include(c => c.Mensajes)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (contacto == null)
        return NotFound();

    ViewBag.ContactoId = contacto.Id;
    return View(contacto);
}

[HttpPost]
public async Task<IActionResult> EnviarMensaje(int ContactoId, string Remitente, string Contenido)
{
    if (string.IsNullOrWhiteSpace(Contenido))
        return RedirectToAction("Chat", new { id = ContactoId });

    var mensaje = new MensajeChat
    {
        ContactoId = ContactoId,
        Remitente = Remitente,
        Contenido = Contenido,
        FechaEnvio = DateTime.UtcNow // ✅ SOLUCIÓN AQUÍ
    };

    _context.DbSetMensajeChat.Add(mensaje);
    await _context.SaveChangesAsync();

    return RedirectToAction("Chat", new { id = ContactoId });
}

        private bool ContactoExists(int id)
        {
            return _context.DbSetContacto.Any(e => e.Id == id);
        }
    }
}
