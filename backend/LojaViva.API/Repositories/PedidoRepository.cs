using LojaViva.API.Data;
using LojaViva.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PedidoRepository> _logger;

        public PedidoRepository(ApplicationDbContext context, ILogger<PedidoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Pedido> GetAll() => _context.Pedidos.Include(p => p.Cliente).ToList();

        public Pedido? GetById(int id) => _context.Pedidos.Include(p => p.Cliente).FirstOrDefault(p => p.Id == id);

        public IEnumerable<Pedido> GetByClienteId(int clienteId) => 
            _context.Pedidos.Where(p => p.ClienteId == clienteId).Include(p => p.Cliente).ToList();

        public void Add(Pedido pedido)
        {
            try
            {
                _logger.LogInformation($"Adicionando pedido para cliente ID: {pedido.ClienteId}");
                _context.Pedidos.Add(pedido);
                _context.SaveChanges();
                _logger.LogInformation($"Pedido adicionado com ID: {pedido.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar pedido: {ex.Message}");
                throw;
            }
        }

        public void Update(Pedido pedido)
        {
            try
            {
                _context.Pedidos.Update(pedido);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar pedido: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var pedido = GetById(id);
                if (pedido != null)
                {
                    _context.Pedidos.Remove(pedido);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir pedido: {ex.Message}");
                throw;
            }
        }
    }
}