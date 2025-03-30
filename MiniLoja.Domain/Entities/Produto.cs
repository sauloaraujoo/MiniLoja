namespace MiniLoja.Domain.Entities
{
    public class Produto : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public decimal Preco { get; set; }
        public int QtdEstoque { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        //TODO: Ligação com o vendedor
    }
}
