using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.App.Models;
using MiniLoja.Core.Data.Context;
using MiniLoja.Core.Domain.Entities;

namespace MiniLoja.App.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly MiniLojaContext _context;

        public CategoriasController(MiniLojaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                .Select(c => new CategoriaViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Descricao = c.Descricao
                }).ToListAsync();

            return View(categorias);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            var viewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var categoria = new Categoria
            {
                Nome = model.Nome,
                Descricao = model.Descricao,
                Ativo = true,
                DataCriacao = DateTime.Now
            };

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            var viewModel = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            };

            ViewBag.Id = categoria.Id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            categoria.Nome = model.Nome;
            categoria.Descricao = model.Descricao;
            categoria.DataAtualizacao = DateTime.Now;

            _context.Update(categoria);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            var viewModel = new CategoriaViewModel
            {
                Nome = categoria.Nome,
                Descricao = categoria.Descricao
            };

            ViewBag.Id = categoria.Id;
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.Produtos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return NotFound();

            if (categoria.Produtos.Any())
            {
                ModelState.AddModelError("", "Não é possível excluir uma categoria com produtos associados.");

                var viewModel = new CategoriaViewModel
                {
                    Id = categoria.Id,
                    Nome = categoria.Nome,
                    Descricao = categoria.Descricao
                };

                return View(viewModel);
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
