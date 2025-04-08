using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniLoja.Api.Models;
using MiniLoja.Business.Vendedores;
using MiniLoja.Domain.Entities;
using MiniLoja.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MiniLoja.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IVendedorRepository _vendedorRepository;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IVendedorRepository vendedorRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _vendedorRepository = vendedorRepository;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(RegisterUserVM registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Username,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password!);

            if (result.Succeeded)
            {
                var vendedor = new Vendedor
                {
                    AspnetUserId = user.Id
                };

                await _vendedorRepository.AdicionarAsync(vendedor);

                await _signInManager.SignInAsync(user, false);
                return Ok(GerarJwt());
            }

            var erros = string.Join(" | ", result.Errors.Select(e => e.Description));

            return Problem("Falha ao registrar o usuário. "  + erros);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserVM loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = await _userManager.FindByEmailAsync(loginUser.Email!);
            if (user == null)
                return Problem("Usuário não encontrado.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginUser.Password!, false, true);

            if (result.Succeeded)
            {
                return Ok(GerarJwt());
            }

            return Problem("Falha ao logar.");
        }

        private string GerarJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}
