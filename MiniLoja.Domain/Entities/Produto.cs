using System.ComponentModel.DataAnnotations;

namespace MiniLoja.Domain.Entities
{
    public class Produto : Entity
    {

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(255)]
        public string Descricao { get; set; }

        [MaxLength(255)]
        public string Imagem { get; set; }

        public decimal Preco { get; set; }

        public int QtdEstoque { get; set; }

        public int CategoriaId { get; set; }

        public int VendedorId { get; set; } 

        public Categoria Categoria { get; set; }

        public Vendedor Vendedor { get; set; } 

    }
}
