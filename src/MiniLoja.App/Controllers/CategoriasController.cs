using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriasController(MiniLojaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return View(_mapper.Map<IEnumerable<CategoriaViewModel>>(categorias));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(_mapper.Map<CategoriaViewModel>(categoria));
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

            var categoria = _mapper.Map<Categoria>(model);
            categoria.Ativo = true;
            categoria.DataCriacao = DateTime.Now;

            _context.Add(categoria);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Categoria criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            ViewBag.Id = categoria.Id;
            return View(_mapper.Map<CategoriaViewModel>(categoria));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            _mapper.Map(model, categoria);
            categoria.DataAtualizacao = DateTime.Now;

            _context.Update(categoria);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Categoria criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            ViewBag.Id = categoria.Id;
            return View(_mapper.Map<CategoriaViewModel>(categoria));
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
                var viewModel = _mapper.Map<CategoriaViewModel>(categoria);
                return View(viewModel);
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Categoria excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}
