using LojaViva.API.Data;
using LojaViva.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProdutoRepository> _logger;

        public ProdutoRepository(ApplicationDbContext context, ILogger<ProdutoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Produto> GetAll() => _context.Produtos.ToList();

        public Produto? GetById(int id) => _context.Produtos.Find(id);

        public void Add(Produto produto)
        {
            try
            {
                _logger.LogInformation($"Adicionando produto: {produto.Nome}");
                _context.Produtos.Add(produto);
                _context.SaveChanges();
                _logger.LogInformation($"Produto adicionado com ID: {produto.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar produto: {ex.Message}");
                throw;
            }
        }

        public void Update(Produto produto)
        {
            try
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar produto: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var produto = GetById(id);
                if (produto != null)
                {
                    _context.Produtos.Remove(produto);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir produto: {ex.Message}");
                throw;
            }
        }
    }
}