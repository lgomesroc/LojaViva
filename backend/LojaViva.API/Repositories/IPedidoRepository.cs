using LojaViva.API.Models;

namespace LojaViva.API.Repositories
{
    public interface IPedidoRepository
    {
        IEnumerable<Pedido> GetAll();
        Pedido? GetById(int id);
        IEnumerable<Pedido> GetByClienteId(int clienteId);
        void Add(Pedido pedido);
        void Update(Pedido pedido);
        void Delete(int id);
    }
}