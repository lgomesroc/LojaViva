namespace LojaViva.API.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; } // Relacionamento
        public DateTime Data { get; set; }
        public decimal Total { get; set; }
    }
}
