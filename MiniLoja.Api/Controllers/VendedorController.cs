using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniLoja.Api.DTOs;
using MiniLoja.Infra.Data.Context;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendedorDto>>> Get()
        {
            var vendedores = await _context.Vendedores
                .Include(v => v.AspnetUser)
                .Select(v => new VendedorDto
                {
                    Id = v.Id,
                    Usuario = v.AspnetUser.UserName!,
                    Email = v.AspnetUser.Email!
                })
                .ToListAsync();

            return Ok(vendedores);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            //TODO:
            return Ok();
        }
    }
}
