using System.ComponentModel.DataAnnotations;

namespace LojaViva.API.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Nome { get; set; }
        
        public string? Descricao { get; set; }
        
        [Required]
        public decimal Preco { get; set; }
        
        public string? ImagemUrl { get; set; }
        
        public int EstoqueQuantidade { get; set; }
        
        public bool Ativo { get; set; } = true;
    }
}