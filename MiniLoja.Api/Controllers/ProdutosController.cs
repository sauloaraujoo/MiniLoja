using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Infra.Data.Context;

namespace MiniLoja.Api.Controllers
{
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
        public async Task<IActionResult> Get()
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .ToListAsync();

            return Ok(produtos);
        }

        [AllowAnonymous]
        [HttpGet("categoria/{idCategoria}")]
        public async Task<IActionResult> GetPorCategoria(int idCategoria)
        {
            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == idCategoria)
                .ToListAsync();

            return Ok(produtos);
        }
    }
}
