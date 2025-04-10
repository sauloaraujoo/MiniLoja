using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Api.DTOs;
using MiniLoja.Domain.Entities;
using MiniLoja.Infra.Data.Context;
using System.Security.Claims;

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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] ProdutoDto produtoDto)
        {
            if (_context.Produtos == null)
            {
                return Problem("Erro ao criar um produto, contate o suporte!");
            }

            if (!ModelState.IsValid)
            {

                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!"
                });
            }

            var categoria = await _context.Categorias.FindAsync(produtoDto.CategoriaId);
            if (categoria == null)
            {
                return NotFound("Categoria não existe.");
            }

            var vendedor = await _context.Vendedores.FindAsync(produtoDto.VendedorId);
            if (vendedor == null)
            {
                return NotFound("Vendedor não existe.");
            }

            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Imagem = produtoDto.Imagem,
                Preco = produtoDto.Preco,
                QtdEstoque = produtoDto.QtdEstoque,
                CategoriaId = produtoDto.CategoriaId,
                DataCriacao = DateTime.Now,
                Ativo = true
            };

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (!await TemPermissaoDoVendedor(produto.VendedorId))
            {
                return Unauthorized();
            }

            return produto;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            return await _context.Produtos
                .ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet("categoria/{idCategoria}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Produto>>> GetPorCategoria(int idCategoria)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            return await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == idCategoria)
                .ToListAsync();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutoDto produtoDto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (!await TemPermissaoDoVendedor(produto.VendedorId))
            {
                return Unauthorized();
            }

            produto.Nome = produtoDto.Nome;
            produto.Descricao = produtoDto.Descricao;
            produto.Imagem = produtoDto.Imagem;
            produto.Preco = produtoDto.Preco;
            produto.QtdEstoque = produtoDto.QtdEstoque;
            produto.DataAtualizacao = DateTime.Now;
            produto.Ativo = true;

            _context.Entry(produto).State = EntityState.Modified;

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
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            if (!await TemPermissaoDoVendedor(produto.VendedorId))
            {
                return Unauthorized();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TemPermissaoDoVendedor(int vendedorId)
        {
            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(v => v.Id == vendedorId);

            if (vendedor == null)
                return false;

            var aspnetUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return vendedor.AspnetUserId == aspnetUserId;
        }
    }
}
