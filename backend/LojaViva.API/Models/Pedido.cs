using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LojaViva.API.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;
        
        public DateTime? DataEntrega { get; set; }
        
        [Required]
        public string Status { get; set; } = "Pendente";
        
        [Required]
        public decimal ValorTotal { get; set; }
        
        [Required]
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        
        public virtual Cliente? Cliente { get; set; }
        
        // Coleção de itens do pedido se necessário
        // public virtual ICollection<ItemPedido>? ItensPedido { get; set; }
    }
}