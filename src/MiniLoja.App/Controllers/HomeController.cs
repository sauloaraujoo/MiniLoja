using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiniLoja.App.Models;
using MiniLoja.Core.Data.Context;
using System.Diagnostics;

namespace MiniLoja.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MiniLojaContext _context;

        public HomeController(ILogger<HomeController> logger, MiniLojaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoriaId)
        {
            var categorias = await _context.Categorias.ToListAsync();
            ViewBag.Categorias = new SelectList(categorias, "Id", "Nome");

            var produtosQuery = _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Ativo && p.DataExclusao == null);

            if (categoriaId.HasValue)
            {
                produtosQuery = produtosQuery.Where(p => p.CategoriaId == categoriaId.Value);
            }

            var produtos = await produtosQuery.ToListAsync();
            return View(produtos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
