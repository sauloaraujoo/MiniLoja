using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Api.DTOs;
using MiniLoja.Core.Domain.Entities;
using MiniLoja.Core.Data.Context;
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
        public async Task<IActionResult> Post([FromForm] ProdutoDto produtoDto)
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
                Imagem = await GetPathImagem(produtoDto.Imagem),
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
        public async Task<IActionResult> Put(int id, [FromForm] ProdutoDto produtoDto)
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
            produto.Imagem = await GetPathImagem(produtoDto.Imagem);
            produto.Preco = produtoDto.Preco;
            produto.QtdEstoque = produtoDto.QtdEstoque;
            produto.DataAtualizacao = DateTime.Now;
            produto.Ativo = true;

            if (produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                DeleteImagem(produto.Imagem);

                produto.Imagem = await GetPathImagem(produtoDto.Imagem);
            }
            else if (string.IsNullOrEmpty(produtoDto.Imagem?.FileName))
            {
                DeleteImagem(produto.Imagem);
                produto.Imagem = string.Empty;
            }

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

            DeleteImagem(produto.Imagem);

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("download/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DownloadImage(int productId)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(productId);

            if (produto == null || string.IsNullOrEmpty(produto.Imagem))
            {
                return NotFound();
            }

            var fullPath = Path.Combine("wwwroot", produto.Imagem);
            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fullPath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(memory, contentType, Path.GetFileName(fullPath));
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

        private async Task<string> GetPathImagem(IFormFile imagem)
        {
            if (imagem != null && imagem.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
                var path = Path.Combine("wwwroot", "Imagens", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                return $"Imagens/{fileName}";
            }

            return string.Empty;
        }

        private void DeleteImagem(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                var fullPath = Path.Combine("wwwroot", imagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }
    }
}
