using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SweetNela.Data;
using SweetNela.Models;
using SweetNela.Service;

namespace SweetNela.Rest
{
    [ApiController]
    [Route("api/producto")]
    public class ProductoApiController : ControllerBase
    {
        private readonly ProductoService _productoService;
        public ProductoApiController(ProductoService service)
        {
            _productoService = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Producto>>> List()
        {
            var productos = await _productoService.GetAll();
            if (productos == null)
                return NotFound();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Producto>> Get(int id)
        {
            var producto = await _productoService.GetById(id);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Create([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest("Invalid product data.");

            var result = await _productoService.Add(producto);
            if (!result)
                return BadRequest("Failed to add product.");

            return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
        }

    }
}