using LojaViva.API.Data;
using LojaViva.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaViva.API.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Pedido> GetAll()
        {
            return _context.Pedidos
                .Include(p => p.Cliente) // Carregar o relacionamento com Cliente
                .ToList();
        }

        public Pedido? GetById(int id)
        {
            return _context.Pedidos
                .Include(p => p.Cliente) // Carregar o relacionamento com Cliente
                .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
        }

        public void Update(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pedido = GetById(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                _context.SaveChanges();
            }
        }
    }
}
