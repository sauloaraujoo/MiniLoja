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
    public class VendedorController : ControllerBase
    {
        private readonly MiniLojaContext _context;

        public VendedorController(MiniLojaContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Vendedor>> GetVendedor(int id)
        {
            if (_context.Vendedores == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedores.FindAsync(id);

            if (vendedor == null)
            {
                return NotFound();
            }

            return vendedor;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<VendedorDto>>> Get()
        {

            if (_context.Vendedores == null)
            {
                return NotFound();
            }

            return await _context.Vendedores
                .Include(v => v.AspnetUser)
                .Select(v => new VendedorDto
                {
                    Id = v.Id,
                    Usuario = v.AspnetUser.UserName!,
                    Email = v.AspnetUser.Email!
                })
                .ToListAsync();
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Vendedores == null)
            {
                return NotFound();
            }

            var vendedor = await _context.Vendedores
                   .Include(v => v.AspnetUser)
                   .FirstOrDefaultAsync(v => v.Id == id);

            if (vendedor == null)
            {
                return NotFound();
            }

            if (vendedor.AspnetUser != null)
            {
                _context.Users.Remove(vendedor.AspnetUser);
            }

            _context.Vendedores.Remove(vendedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
