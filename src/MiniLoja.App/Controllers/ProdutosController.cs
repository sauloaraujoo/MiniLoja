using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniLoja.App.Models;
using MiniLoja.Core.Data.Context;
using MiniLoja.Core.Domain.Entities;

namespace MiniLoja.App.Controllers
{
    [Authorize]
    public class ProdutosController : Controller
    {
        private readonly MiniLojaContext _context;
        private readonly IWebHostEnvironment _env;

        public ProdutosController(MiniLojaContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        public async Task<IActionResult> Index()
        {
            var aspnetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!TemPermissaoDoVendedor(aspnetUserId)) return Unauthorized();

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(v => v.AspnetUserId == aspnetUserId);

            if (vendedor == null) return Unauthorized();

            var produtos = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.VendedorId == vendedor.Id)
                .ToListAsync();

            return View(produtos);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null || !TemPermissaoDoVendedor(produto.Vendedor.AspnetUserId)) return Unauthorized();

            return View(produto);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao");
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "AspnetUserId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Descricao", model.CategoriaId);
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vendedor = await _context.Vendedores.FirstOrDefaultAsync(v => v.AspnetUserId == userId);

            if (vendedor == null) return Unauthorized();

            var imagemPath = await SalvarImagem(model.Imagem);

            var produto = new Produto
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Imagem = imagemPath,
                Preco = model.Preco,
                QtdEstoque = model.QtdEstoque,
                CategoriaId = model.CategoriaId,
                VendedorId = vendedor.Id
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (produto == null) return NotFound();

            if (!TemPermissaoDoVendedor(produto.Vendedor.AspnetUserId)) return Unauthorized();

            var model = new ProdutoViewModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                QtdEstoque = produto.QtdEstoque,
                CategoriaId = produto.CategoriaId,
                VendedorId = produto.VendedorId,
                ImagemPath = produto.Imagem
            };

            ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Descricao", model.CategoriaId);
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "AspnetUserId", produto.VendedorId);

            return View(model);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["CategoriaId"] = new SelectList(await _context.Categorias.ToListAsync(), "Id", "Descricao", model.CategoriaId);
                return View(model);
            }

            var produto = await _context.Produtos
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (produto == null || !TemPermissaoDoVendedor(produto.Vendedor.AspnetUserId)) return Unauthorized();

            produto.Nome = model.Nome;
            produto.Descricao = model.Descricao;
            produto.Preco = model.Preco;
            produto.QtdEstoque = model.QtdEstoque;
            produto.CategoriaId = model.CategoriaId;

            if (model.Imagem != null)
            {
                RemoverImagem(produto.Imagem);
                produto.Imagem = await SalvarImagem(model.Imagem);
            }

            _context.Update(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null || !TemPermissaoDoVendedor(produto.Vendedor.AspnetUserId)) return Unauthorized();

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await _context.Produtos
                .Include(p => p.Vendedor)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (produto == null || !TemPermissaoDoVendedor(produto.Vendedor.AspnetUserId)) return Unauthorized();

            RemoverImagem(produto.Imagem);

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TemPermissaoDoVendedor(string aspnetUserId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return aspnetUserId == userId;
        }

        private async Task<string> SalvarImagem(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0) return null;

            var pasta = Path.Combine(_env.WebRootPath, "Imagens");
            Directory.CreateDirectory(pasta);

            var nomeArquivo = Guid.NewGuid() + Path.GetExtension(imagem.FileName);
            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            return Path.Combine("Imagens", nomeArquivo).Replace("\\", "/");
        }

        private void RemoverImagem(string caminhoRelativo)
        {
            if (string.IsNullOrEmpty(caminhoRelativo)) return;

            var caminho = Path.Combine(_env.WebRootPath, caminhoRelativo);
            if (System.IO.File.Exists(caminho))
            {
                System.IO.File.Delete(caminho);
            }
        }
    }
}
