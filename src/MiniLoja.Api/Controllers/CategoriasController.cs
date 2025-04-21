using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Api.DTOs;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;

namespace MiniLoja.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly MiniLojaContext _context;

        public CategoriasController(MiniLojaContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post([FromBody] CategoriaDto categoriaDto)
        {
            if (_context.Categorias == null)
            {
                return Problem("Erro ao criar um categoria, contate o suporte!");
            }

            if (!ModelState.IsValid)
            {

                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            var categoria = new Categoria
            {
                Nome = categoriaDto.Nome,
                Descricao = categoriaDto.Descricao,
                DataCriacao = DateTime.Now,
                Ativo = true
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            return await _context.Categorias.ToListAsync();
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaDto categoriaDto)
        {

            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Nome = categoriaDto.Nome;
            categoria.Descricao = categoriaDto.Descricao;
            categoria.DataAtualizacao = DateTime.Now;
            categoria.Ativo = true;

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            if (categoria.Produtos.Any())
            {
                return BadRequest("Não permite exclusão, pois há produto(s) associado(s).");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
