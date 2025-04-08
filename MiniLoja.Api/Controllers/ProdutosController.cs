using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Api.DTOs;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly MiniLojaContext _context;

        public ProdutosController(MiniLojaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProdutoDto produtoDto)
        {
            //TODO:
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _context.Produtos
                //.Include(p => p.Categoria)
                .ToListAsync();

            return Ok(produtos);
        }

        [AllowAnonymous]
        [HttpGet("categoria/{idCategoria}")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPorCategoria(int idCategoria)
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == idCategoria)
                .ToListAsync();

            return Ok(produtos);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProdutoDto produtoDto)
        {
            //TODO:
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            //TODO:
            return Ok();
        }
    }
}
