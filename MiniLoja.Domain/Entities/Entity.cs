using System.ComponentModel.DataAnnotations;

namespace MiniLoja.Domain.Entities
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; } 
        public bool Ativo { get; set; } = true;   
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao {  get; set; }

    }
}
