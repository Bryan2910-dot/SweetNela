using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SweetNela.Models;
using Microsoft.AspNetCore.Identity;
using SweetNela.Data;

namespace SweetNela.Controllers
{
    public class ContactoController : Controller
    {
        private readonly ILogger<ContactoController> _logger;
        private readonly ApplicationDbContext _context;


        private readonly UserManager<IdentityUser> _userManager;

        public ContactoController(ILogger<ContactoController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
    _logger = logger;
    _userManager = userManager;
    _context = context;
}


        public async Task<IActionResult> Index()
{
    var contacto = new Contacto();

    if (User.Identity != null && User.Identity.IsAuthenticated)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            contacto.Email = user.Email;
            contacto.Telefono = user.PhoneNumber; 
        }
    }

    return View(contacto);
}
        
        [HttpPost]
        public IActionResult RegistrarContacto(Contacto contacto)          
        {
            if (ModelState.IsValid)
            {
                contacto.Respuesta = "Datos correctamente enviados"; 
                _context.DbSetContacto.Add(contacto);
                _context.SaveChanges();
                _logger.LogInformation("Registrado su contacto");
                return View("Index", contacto); 
            }
            else
            {
                return View("Index", contacto); 
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}