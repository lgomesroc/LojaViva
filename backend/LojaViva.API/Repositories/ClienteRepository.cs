using LojaViva.API.Data;
using LojaViva.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClienteRepository> _logger;

        public ClienteRepository(ApplicationDbContext context, ILogger<ClienteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Cliente> GetAll() => _context.Clientes.ToList();

        public Cliente? GetById(int id) => _context.Clientes.Find(id);

        public Cliente? GetByEmail(string email)
        {
            return _context.Clientes.FirstOrDefault(c => c.Email == email);
        }

        public void Add(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar cliente: {ex.Message}");
                throw; // Re-lança a exceção para ser tratada no controller
            }
        }

        public void Update(Cliente cliente)
        {
            try
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar cliente: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var cliente = GetById(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir cliente: {ex.Message}");
                throw;
            }
        }

        public bool ValidateCredentials(string email, string senha)
        {
            var cliente = GetByEmail(email);
            // Verifica se o cliente existe e se a senha corresponde
            return cliente != null && cliente.Senha == senha;
        }
    }
}