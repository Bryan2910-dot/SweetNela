using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SweetNela.Data;
using SweetNela.Dto;
using SweetNela.Integration.Exchange;
using SweetNela.Models;
using Microsoft.AspNetCore.Authorization;

namespace SweetNela.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public class CatalogoApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetProductos()
        {
            var productos = _context.DbSetProducto.ToList();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetProducto(int id)
        {
            var producto = _context.DbSetProducto.Find(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
    }
}