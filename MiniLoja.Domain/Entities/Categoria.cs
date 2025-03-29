namespace MiniLoja.Domain.Entities
{
    public class Categoria : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<Produto> Produtos { get; set; }
    }
}
