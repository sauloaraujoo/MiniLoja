using System.ComponentModel.DataAnnotations;

namespace MiniLoja.Core.Domain.Entities
{
    public class Categoria : Entity
    {
        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(255)]
        public string Descricao { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}
