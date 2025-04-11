using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMemoryCache _cache;

        public ProdutoController(IProdutoRepository produtoRepository, ILogger<ProdutoController> logger, IMemoryCache cache)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            _logger.LogInformation("GET /api/produtos chamado");

            // Tenta obter produtos do cache
            if (!_cache.TryGetValue("ProdutosCache", out List<Produto> produtos))
            {
                _logger.LogInformation("Cache vazio. Recuperando produtos do repositório.");

                // Recuperar dados do repositório
                produtos = _produtoRepository.GetAll().ToList();

                // Configurar opções de cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                // Adicionar produtos ao cache
                _cache.Set("ProdutosCache", produtos, cacheOptions);
                _logger.LogInformation("Produtos adicionados ao cache.");
            }
            else
            {
                _logger.LogInformation("Produtos recuperados do cache.");
            }

            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"GET /api/produtos/{id} chamado");
            var produto = _produtoRepository.GetById(id);
            if (produto == null)
            {
                _logger.LogWarning($"Produto com ID {id} não encontrado");
                return NotFound();
            }
            _logger.LogInformation($"Produto com ID {id} encontrado: Nome - {produto.Nome}");
            return Ok(produto);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Produto produto)
        {
            _logger.LogInformation($"POST /api/produtos chamado para adicionar produto: {produto.Nome}");
            _produtoRepository.Add(produto);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ProdutosCache");

            _logger.LogInformation($"Produto {produto.Nome} adicionado com sucesso, ID: {produto.Id}");
            return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Produto produto)
        {
            _logger.LogInformation($"PUT /api/produtos/{id} chamado para atualizar produto.");
            var existingProduto = _produtoRepository.GetById(id);
            if (existingProduto == null)
            {
                _logger.LogWarning($"Produto com ID {id} não encontrado para atualização.");
                return NotFound();
            }

            produto.Id = id;
            _produtoRepository.Update(produto);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ProdutosCache");

            _logger.LogInformation($"Produto com ID {id} atualizado com sucesso.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE /api/produtos/{id} chamado para excluir produto.");
            var produto = _produtoRepository.GetById(id);
            if (produto == null)
            {
                _logger.LogWarning($"Produto com ID {id} não encontrado para exclusão.");
                return NotFound();
            }

            _produtoRepository.Delete(id);

            // Invalida o cache para forçar a atualização
            _cache.Remove("ProdutosCache");

            _logger.LogInformation($"Produto com ID {id} excluído com sucesso.");
            return NoContent();
        }
    }
}
