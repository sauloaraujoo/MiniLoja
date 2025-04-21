using Microsoft.AspNetCore.Identity;

namespace MiniLoja.Core.Domain.Entities
{
    public class Vendedor : Entity
    {
        public string AspnetUserId { get; set; }

        public IdentityUser AspnetUser { get; set; }

        public ICollection<Produto> Produtos { get; set; }

    }
}
