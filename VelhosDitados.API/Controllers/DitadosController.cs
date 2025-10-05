using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VelhosDitados.API.Data;
using VelhosDitados.API.DTOs;
using VelhosDitados.API.Models;

namespace VelhosDitados.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DitadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DitadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ditados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ditado>>> GetDitados()
        {
            return await _context.Ditados.ToListAsync();
        }

        // GET: api/ditados/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Ditado>> GetDitado(Guid id)
        {
            var ditado = await _context.Ditados.FindAsync(id);

            if (ditado == null)
                return NotFound();

            return ditado;
        }

        // GET: api/ditados/aleatorio
        [HttpGet("aleatorio")]
        public async Task<IActionResult> GetAleatorio()
        {
            try
            {
                var ditados = await _context.Ditados.ToListAsync();
                if (!ditados.Any())
                    return NotFound("Nenhum ditado encontrado.");

                var random = new Random();
                var index = random.Next(ditados.Count);
                var ditadoAleatorio = ditados[index];                
                return Ok(ditadoAleatorio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro interno no servidor", detalhes = ex.Message });
            }
        }


        // POST: api/ditados
        [HttpPost]
        public async Task<ActionResult<Ditado>> PostDitado(DitadoCreateDTO dto)
        {
            var ditado = new Ditado
            {
                Descricao = dto.Descricao
            };

            _context.Ditados.Add(ditado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDitado), new { id = ditado.Id }, ditado);
        }

        // PUT: api/ditados/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDitado(Guid id, Ditado ditado)
        {
            if (id != ditado.Id)
                return BadRequest();

            _context.Entry(ditado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ditados.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ditados/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDitado(Guid id)
        {
            var ditado = await _context.Ditados.FindAsync(id);
            if (ditado == null)
                return NotFound();

            _context.Ditados.Remove(ditado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
