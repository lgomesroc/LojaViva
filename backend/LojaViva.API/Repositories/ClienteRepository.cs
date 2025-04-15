using LojaViva.API.Data;
using LojaViva.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaViva.API.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente> GetAll() => _context.Clientes.ToList();

        public Cliente? GetById(int id) => _context.Clientes.Find(id);

        public Cliente? GetByEmail(string email)
        {
            return _context.Clientes.FirstOrDefault(c => c.Email == email);
        }

        public void Add(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void Update(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cliente = GetById(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
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