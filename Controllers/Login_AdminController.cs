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
    public class Login_AdminController : Controller
    {
        private readonly ILogger<Login_AdminController> _logger;

        public Login_AdminController(ILogger<Login_AdminController> logger)
        {
            _logger = logger;
        }

    
        public IActionResult Index()
        {
            return View(new Login_Admin());
        }

        [HttpPost]
        public IActionResult Login(Login_Admin model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (model.AdminNombre == "admin" && model.AdminContra == "123456")
            {
                return RedirectToAction("Index", "Admin");
            }


            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View("Index", model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
