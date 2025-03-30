using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
