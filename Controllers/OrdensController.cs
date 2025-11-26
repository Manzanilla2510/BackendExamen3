using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Examen3.Data;
using Examen3.Entidades;
using System.Net.Http;
using System.Text.Json;

namespace Examen3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdensController : ControllerBase
    {
        private readonly Examen3Context _context;
        private readonly IHttpClientFactory _httpFactory;

        public OrdensController(Examen3Context context, IHttpClientFactory httpFactory)
        {
            _context = context;
            _httpFactory = httpFactory;
        }

        // GET: api/Ordens
        // Devuelve órdenes con calificación >= 100
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrden()
        {
            return await _context.Orden
                                 .Where(o => o.calificacion >= 100)
                                 .ToListAsync();
        }

        // GET: api/Ordens/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Orden>> GetOrden(int id)
        {
            var orden = await _context.Orden.FindAsync(id);
            if (orden == null)
                return NotFound();
            return orden;
        }

        // GET: api/Ordens/ProveedoresRemotos
        // Consume la API remota y devuelve los datos
        [HttpGet("ProveedoresRemotos")]
        public async Task<IActionResult> ProveedoresRemotos()
        {
            var client = _httpFactory.CreateClient("Api1");

            var response = await client.GetAsync("api/Proveedores/Listar");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Error al consultar la API remota");

            var contenido = await response.Content.ReadAsStringAsync();

            // Deserializar a un objeto dinámico para poder manipularlo si quieres
            var datos = JsonSerializer.Deserialize<object>(contenido, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(datos);
        }

        // POST: api/Ordens
        [HttpPost]
        public async Task<ActionResult<Orden>> PostOrden(Orden orden)
        {
            _context.Orden.Add(orden);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrden), new { id = orden.Id }, orden);
        }

        // PUT: api/Ordens/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrden(int id, Orden orden)
        {
            if (id != orden.Id)
                return BadRequest();

            _context.Entry(orden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Ordens/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrden(int id)
        {
            var orden = await _context.Orden.FindAsync(id);
            if (orden == null)
                return NotFound();

            _context.Orden.Remove(orden);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrdenExists(int id)
        {
            return _context.Orden.Any(e => e.Id == id);
        }
    }
}
