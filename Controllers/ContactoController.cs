using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sweetnela.Models;
using Microsoft.AspNetCore.Identity;

namespace Sweetnela.Controllers
{
    public class ContactoController : Controller
    {
        private readonly ILogger<ContactoController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public ContactoController(ILogger<ContactoController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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