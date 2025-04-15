using System.ComponentModel.DataAnnotations;

namespace LojaViva.API.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Nome { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        public string? Telefone { get; set; }
        
        public string? Endereco { get; set; }
        
        [Required]
        public required string Senha { get; set; }
    }
}