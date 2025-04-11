using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LojaViva.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ILogger<PedidoController> _logger;
        private readonly IMemoryCache _cache;

        public PedidoController(IPedidoRepository pedidoRepository, ILogger<PedidoController> logger, IMemoryCache cache)
        {
            _pedidoRepository = pedidoRepository;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult GetPedidos()
        {
            _logger.LogInformation("GET /api/pedidos chamado");

            // Tenta obter pedidos do cache
            if (!_cache.TryGetValue("PedidosCache", out List<Pedido> pedidos))
            {
                _logger.LogInformation("Cache vazio. Recuperando pedidos do repositório.");

                // Recuperar dados do repositório
                pedidos = _pedidoRepository.GetAll().ToList();

                // Configuração de opções de cache
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                // Adicionar pedidos ao cache
                _cache.Set("PedidosCache", pedidos, cacheOptions);
                _logger.LogInformation("Pedidos adicionados ao cache.");
            }
            else
            {
                _logger.LogInformation("Pedidos recuperados do cache.");
            }

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPedido(int id)
        {
            _logger.LogInformation($"GET /api/pedidos/{id} chamado");
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null)
            {
                _logger.LogWarning($"Pedido com ID {id} não encontrado");
                return NotFound(new { Message = "Pedido não encontrado." });
            }
            _logger.LogInformation($"Pedido com ID {id} encontrado: Total - {pedido.Total}");
            return Ok(pedido);
        }

        [HttpPost]
        public IActionResult AddPedido([FromBody] Pedido pedido)
        {
            _logger.LogInformation($"POST /api/pedidos chamado para adicionar pedido com Total: {pedido.Total}");
            _pedidoRepository.Add(pedido);

            // Invalida o cache para forçar a atualização
            _cache.Remove("PedidosCache");

            _logger.LogInformation($"Pedido adicionado com sucesso, ID: {pedido.Id}");
            return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePedido(int id, [FromBody] Pedido pedido)
        {
            _logger.LogInformation($"PUT /api/pedidos/{id} chamado para atualizar pedido.");
            var pedidoExistente = _pedidoRepository.GetById(id);
            if (pedidoExistente == null)
            {
                _logger.LogWarning($"Pedido com ID {id} não encontrado para atualização.");
                return NotFound(new { Message = "Pedido não encontrado." });
            }

            pedido.Id = id;
            _pedidoRepository.Update(pedido);

            // Invalida o cache para forçar a atualização
            _cache.Remove("PedidosCache");

            _logger.LogInformation($"Pedido com ID {id} atualizado com sucesso.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePedido(int id)
        {
            _logger.LogInformation($"DELETE /api/pedidos/{id} chamado para excluir pedido.");
            var pedido = _pedidoRepository.GetById(id);
            if (pedido == null)
            {
                _logger.LogWarning($"Pedido com ID {id} não encontrado para exclusão.");
                return NotFound(new { Message = "Pedido não encontrado." });
            }

            _pedidoRepository.Delete(id);

            // Invalida o cache para forçar a atualização
            _cache.Remove("PedidosCache");

            _logger.LogInformation($"Pedido com ID {id} excluído com sucesso.");
            return NoContent();
        }
    }
}
