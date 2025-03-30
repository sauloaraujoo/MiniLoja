using System.ComponentModel.DataAnnotations;

namespace MiniLoja.Api.Models
{
    public class LoginUserVM
    {
        [Required(ErrorMessage = "Campo {0} obrigatório.")]
        [EmailAddress(ErrorMessage = "Campo {0} está em formato inválido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Campo {0} obrigatório.")]
        [StringLength(100, ErrorMessage = "Campo {0} precisa ter entre {1} e {2} caracteres.", MinimumLength = 6)]
        public string? Password { get; set; }
    }
}
